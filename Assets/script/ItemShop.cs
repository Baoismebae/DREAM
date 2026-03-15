using UnityEngine;

public class ItemShop : MonoBehaviour
{
    public enum ItemType { Health, Mana, Damage } // Danh sách loại đồ
    
    [Header("Cài đặt món đồ")]
    public ItemType loaiDo; 
    public int giaTien = 10;
    public float giaTriTang = 20f; // Hồi bao nhiêu máu/mana hoặc tăng bao nhiêu dame

    private bool isPlayerNearby = false;

    void Update()
    {
        // Nếu Player đứng gần và nhấn phím E (hoặc phím cậu chọn)
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            BuyItem();
        }
    }

    void BuyItem()
    {
        // Kiểm tra ví tiền từ PlayerStats
        if (PlayerStats.instance.SpendCoins(giaTien))
        {
            // Nếu đủ tiền, thực hiện logic tăng chỉ số
            switch (loaiDo)
            {
                case ItemType.Health:
                    Debug.Log("WaiiBi đã mua Máu! + " + giaTriTang);
                    // Gọi hàm hồi máu: PlayerHealth.instance.Heal(giaTriTang);
                    break;
                case ItemType.Mana:
                    Debug.Log("WaiiBi đã mua Mana! + " + giaTriTang);
                    // Gọi hàm hồi mana: PlayerMana.instance.Refill(giaTriTang);
                    break;
                case ItemType.Damage:
                    Debug.Log("WaiiBi đã tăng Sức mạnh! + " + giaTriTang);
                    // Gọi hàm tăng dame: PlayerCombat.instance.IncreaseDamage(giaTriTang);
                    break;
            }
        }
    }

    // Kiểm tra Mage có đang đứng ở bàn không
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = false;
    }
}