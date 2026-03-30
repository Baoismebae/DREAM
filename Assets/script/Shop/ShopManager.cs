using UnityEngine;
using TMPro;

public enum ItemType { HealthPotion, SpeedBowl, DefenseFood }

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;
    
    [Header("UI & Tiền")]
    public TextMeshProUGUI boardText;
    public int playerGold = 50;

    // --- PHẦN THÊM MỚI CHO ÂM THANH ---
    [Header("Âm thanh")]
    public AudioSource audioSource;
    public AudioClip buySuccessSound; // Tiếng khi mua được
    public AudioClip buyFailSound;    // Tiếng khi thiếu tiền
    // ----------------------------------

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateBoard("WELCOME :3");
    }

    public void UpdateBoard(string message)
    {
        boardText.text = message;
    }

    public void TryBuyItem(string itemName, int price, ItemType type)
    {
        if (playerGold >= price)
        {
            playerGold -= price;
            UpdateBoard("You bought " + itemName + " :)");
            
            // --- THÊM DÒNG NÀY ĐỂ PHÁT NHẠC THÀNH CÔNG ---
            if (audioSource != null && buySuccessSound != null)
            {
                audioSource.PlayOneShot(buySuccessSound);
            }

            ApplyItemEffect(type);
        }
        else
        {
            UpdateBoard("Not enough coin :(");
            
            // --- THÊM DÒNG NÀY ĐỂ PHÁT NHẠC THẤT BẠI ---
            if (audioSource != null && buyFailSound != null)
            {
                audioSource.PlayOneShot(buyFailSound);
            }
        }
    }

    private void ApplyItemEffect(ItemType type)
    {
        // (Giữ nguyên phần này như cũ nhé)
        switch (type)
        {
            case ItemType.HealthPotion:
                Debug.Log("Đã mua hồi máu cho người chơi!");
                break;
            case ItemType.SpeedBowl:
                Debug.Log("Đã mua Tăng tốc độ chạy!");
                break;
            case ItemType.DefenseFood:
                Debug.Log("Đã mua Tăng phòng thủ!");
                break;
        }
    }
}