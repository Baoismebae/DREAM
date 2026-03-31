using UnityEngine;

public class ShopItem : MonoBehaviour
{
    // Cậu chỉ cần kéo file Data (ví dụ: file LoHoiMau) từ cửa sổ Project vào đây!
    public ItemData itemData;

    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShopManager.Instance.TryBuyItem(itemData);
        }
    }

    void OnMouseDown()
    {
        if (isPlayerInRange)
        {
            ShopManager.Instance.TryBuyItem(itemData);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            ShopManager.Instance.UpdateBoard("Press [ E ] to buy");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            ShopManager.Instance.UpdateBoard("WELCOME :3");
        }
    }
}