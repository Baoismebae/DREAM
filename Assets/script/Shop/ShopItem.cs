using UnityEngine;

public class ShopItem : MonoBehaviour
{
    // Kéo file Data (ví dụ: file LoHoiMau) từ cửa sổ Project vào đây!
    [Header("Item Info")]
    public ItemData itemData;

    private bool isPlayerInRange = false;
    private bool isSold = false; // Trạng thái kiểm tra xem món đồ đã bị mua chưa

    void Update()
    {
        // Thêm điều kiện !isSold để không cho mua lại món đã bán
        if (isPlayerInRange && !isSold && Input.GetKeyDown(KeyCode.E))
        {
            AttemptPurchase();
        }
    }

    void OnMouseDown()
    {
        if (isPlayerInRange && !isSold)
        {
            AttemptPurchase();
        }
    }

    // Tách riêng logic mua hàng ra một hàm để dùng chung cho cả phím E và Chuột
    private void AttemptPurchase()
    {
        // Cập nhật hàm TryBuyItem bên ShopManager để trả về true/false
        // true = Đủ tiền & Túi đồ còn chỗ trống -> Mua thành công
        bool success = ShopManager.Instance.TryBuyItem(itemData);

        if (success)
        {
            isSold = true;
            isPlayerInRange = false; // Reset trạng thái để không lỗi bảng UI

            // Cập nhật bảng thông báo báo hết hàng
            ShopManager.Instance.UpdateBoard("SOLD OUT!");

            // Tắt hình ảnh món đồ trên quầy (ẩn đi thay vì Destroy để tránh lỗi Missing Reference)
            gameObject.SetActive(false);
        }
        else
        {
            // Báo lỗi nếu không đủ tiền hoặc túi đầy
            ShopManager.Instance.UpdateBoard("INVENTORY FULL / NO COINS!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isSold)
        {
            isPlayerInRange = true;
            // Bạn có thể lấy giá tiền từ itemData để hiển thị lên bảng luôn cho trực quan
            ShopManager.Instance.UpdateBoard("Press [ E ] to buy - " + itemData.price + "G");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Chỉ trả về WELCOME nếu đồ chưa bị mua
            if (!isSold)
            {
                ShopManager.Instance.UpdateBoard("WELCOME :3");
            }
        }
    }
}