using UnityEngine;
using TMPro; // 1. BẮT BUỘC phải có dòng này nhé 
using System.Collections.Generic; // BẮT BUỘC phải có dòng này để dùng Dictionary (Túi đồ)

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [Header("Cài đặt tiền tệ")]
    public int currentCoins = 0;

    [Header("Giao diện UI")]
    public TextMeshProUGUI coinTextDisplay; // 2. Chỗ để kéo cái CoinText vào

    [Header("TÚI ĐỒ (INVENTORY)")]
    // Key: Loại vật phẩm (Health, Speed, Shield), Value: Số lượng đang có
    public Dictionary<ItemData.ItemType, int> inventory = new Dictionary<ItemData.ItemType, int>();

    void Awake()
    {
        instance = this;

        // Khởi tạo túi đồ trống rỗng cho cả 3 loại lọ lúc mới chơi
        inventory[ItemData.ItemType.Health] = 0;
        inventory[ItemData.ItemType.Speed] = 0;
        inventory[ItemData.ItemType.Shield] = 0;
    }

    void Start()
    {
        UpdateCoinUI(); // Cập nhật chữ lúc mới vào game
    }

    // ==========================================
    // QUẢN LÝ TIỀN BẠC
    // ==========================================
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        Debug.Log("nhặt được " + amount + " xu! Tổng ví: " + currentCoins);
        UpdateCoinUI(); // 3. Cập nhật lại UI mỗi khi nhặt tiền
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            Debug.Log("Bạn đã mua đồ! Còn lại: " + currentCoins);
            UpdateCoinUI(); // 3. Cập nhật lại UI mỗi khi tiêu tiền
            return true;
        }
        return false;
    }

    // ĐÂY LÀ HÀM CẬU ĐANG TÌM:
    void UpdateCoinUI()
    {
        if (coinTextDisplay != null)
        {
            // Cậu dùng <sprite=0> như mình đã cài đặt lúc nãy nhé
            coinTextDisplay.text = currentCoins.ToString() + " <sprite=0 voffset=0.8em>";
        }
    }

    // ==========================================
    // MUA ĐỒ TỪ SHOP BỆ ĐÁ
    // ==========================================
    public bool TryBuyItem(ItemData itemData)
    {
        // Dùng luôn hàm SpendCoins cậu đã viết ở trên để check và trừ tiền
        if (SpendCoins(itemData.cost))
        {
            // Mua thành công -> Thêm 1 vật phẩm vào túi đồ
            inventory[itemData.type]++;

            // Phát tiếng mua thành công (Keng keng!)
            if (GlobalAudioManager.Instance != null)
                GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.buySuccessSound);

            Debug.Log("Mua thành công: " + itemData.itemName + ". Số lượng lọ " + itemData.type + " đang có: " + inventory[itemData.type]);
            return true;
        }
        else
        {
            // Hết tiền, phát tiếng bíp trầm báo lỗi
            if (GlobalAudioManager.Instance != null)
                GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.buyFailSound);

            Debug.LogWarning("Không đủ tiền mua: " + itemData.itemName + " nha bạn ơi!");
            return false;
        }
    }
}