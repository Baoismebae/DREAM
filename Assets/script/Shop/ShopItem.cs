using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public string itemName;
    public int price = 10;
    public ItemType itemType;

    private bool isPlayerInRange = false;

    void Update()
    {
        // Nhấn nút E để mua nếu đang đứng gần
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShopManager.Instance.TryBuyItem(itemName, price, itemType);
        }
    }

    // Click chuột trái để mua (Yêu cầu phải có Collider2D trên vật phẩm)
    void OnMouseDown()
    {
        // Kiểm tra xem người chơi có đang đứng gần không, nếu không muốn giới hạn khoảng cách click thì xóa dòng if này đi
        if (isPlayerInRange)
        {
            ShopManager.Instance.TryBuyItem(itemName, price, itemType);
        }
    }

    // Khi người chơi đi vào vùng của món đồ
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Hiện hướng dẫn lên bảng trắng
            ShopManager.Instance.UpdateBoard("Press [ E ] to buy");
        }
    }

    // Khi người chơi rời đi
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Xóa hướng dẫn, đưa bảng về trạng thái chào mừng
            ShopManager.Instance.UpdateBoard("WELCOME :3");
        }
        Debug.Log("Có vật chạm vào món đồ: " + other.gameObject.name);
    }
}