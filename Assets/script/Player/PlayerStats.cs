using UnityEngine;
using TMPro; // 1. BẮT BUỘC phải có dòng này nhé WaiiBi!

public class PlayerStats : MonoBehaviour
{
    [Header("Cài đặt tiền tệ")]
    public int currentCoins = 0; 
    public static PlayerStats instance; 

    [Header("Giao diện UI")]
    public TextMeshProUGUI coinTextDisplay; // 2. Chỗ để kéo cái CoinText vào

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateCoinUI(); // Cập nhật chữ lúc mới vào game
    }

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
            Debug.Log("WaiiBi đã mua đồ! Còn lại: " + currentCoins);
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
}