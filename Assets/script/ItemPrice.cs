using UnityEngine;
using TMPro; // Nếu cậu dùng TextMeshPro

public class ItemPrice : MonoBehaviour
{
    public GameObject priceUI; // Kéo cái PriceTag vào đây

    // Khi nhân vật bước vào vùng hình tròn
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            priceUI.SetActive(true); // Hiện giá tiền
        }
    }

    // Khi nhân vật đi ra xa
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            priceUI.SetActive(false); // Ẩn giá tiền
        }
    }
}