using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [Header("Dữ liệu món hàng")]
    public ItemData itemData; // Kéo thả file Thịt, Cháo, Máu vào đây

    private bool isPlayerInRange = false;

    void Update()
    {
        // Bấm E để mua khi đứng gần
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            AttemptPurchase();
        }
    }

    void OnMouseDown()
    {
        // Click chuột để mua (phương án dự phòng)
        if (isPlayerInRange)
        {
            AttemptPurchase();
        }
    }

    private void AttemptPurchase()
    {
        if (itemData == null)
        {
            Debug.LogWarning("Món đồ này chưa được gắn file ItemData!");
            return;
        }

        if (ShopManager.Instance == null) return;

        // Báo cho ShopManager xử lý mua hàng
        bool success = ShopManager.Instance.TryBuyItem(itemData);

        if (success)
        {
            // Mua thành công -> Hiện chữ báo cáo
            ShopManager.Instance.UpdateBoard("BOUGHT " + itemData.itemName + ":)");
            // Đã bỏ lệnh Destroy để đồ không bị mất khỏi bàn
        }
        else
        {
            // Tiền không đủ hoặc túi đầy
            ShopManager.Instance.UpdateBoard("NOT ENOUGH COINS :(");
        }
    }

    // BƯỚC VÀO VÙNG CẢM BIẾN
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (ShopManager.Instance != null && itemData != null)
            {
                ShopManager.Instance.UpdateBoard("Press [ E ] to buy");
            }
        }
    }

    // BƯỚC RA KHỎI VÙNG CẢM BIẾN
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (ShopManager.Instance != null)
            {
                ShopManager.Instance.UpdateBoard("WELCOME :3");
            }
        }
    }
}