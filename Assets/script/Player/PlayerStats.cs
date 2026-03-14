using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Cài đặt tiền tệ")]
    public int currentCoins = 0; // Số tiền hiện có

    // Hàm này để script khác (như Coin) gọi tới khi nhặt được tiền
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        Debug.Log("nhặt được " + amount + " xu! Tổng ví: " + currentCoins);
    }

    // Hàm này để Cửa hàng gọi tới khi mua đồ
    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            return true; // Đủ tiền mua
        }
        return false; // Nghèo quá không mua được
    }
}