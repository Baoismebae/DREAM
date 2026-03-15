using UnityEngine;

public class ItemShop : MonoBehaviour
{
    public enum ItemType { Health, Mana, Damage } // Các loại món đồ

    [Header("Cài đặt món đồ")]
    public ItemType loaiMonDo; 
    public int giaTien = 10;
    public float giaTriCong = 20f; // Máu/Mana hồi bao nhiêu, hoặc Dame tăng bao nhiêu

    [Header("Giao diện")]
    public GameObject thongBaoText; // Cái chữ "Nhấn E để mua"

    private bool isPlayerNearby = false;

    void Start()
    {
        if(thongBaoText != null) thongBaoText.SetActive(false); // Lúc đầu ẩn thông báo đi
    }

    void Update()
    {
        // Nếu Mage đứng gần và nhấn phím E
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Buy();
        }
    }

    void Buy()
    {
        // Gọi ví tiền của Mage để kiểm tra
        if (PlayerStats.instance.SpendCoins(giaTien))
        {
            // Nếu đủ tiền thì thực hiện hành động tương ứng
            switch (loaiMonDo)
            {
                case ItemType.Health:
                    Debug.Log("Đã hồi Máu!");
                    // Gọi hàm hồi máu của Mage ở đây: PlayerHealth.instance.Heal(giaTriCong);
                    break;
                case ItemType.Mana:
                    Debug.Log("Đã hồi Mana!");
                    break;
                case ItemType.Damage:
                    Debug.Log("Đã tăng Sức mạnh!");
                    break;
            }
        }
        else
        {
            Debug.Log("WaiiBi ơi, không đủ tiền rồi!");
        }
    }

    // Khi Mage bước vào vùng của bàn
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            if(thongBaoText != null) thongBaoText.SetActive(true);
        }
    }

    // Khi Mage đi ra xa khỏi bàn
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            if(thongBaoText != null) thongBaoText.SetActive(false);
        }
    }
}