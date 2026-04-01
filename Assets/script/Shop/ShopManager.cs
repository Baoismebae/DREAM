using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("UI Bảng Hướng Dẫn")]
    public TextMeshProUGUI boardText; // Nơi chứa dòng chữ "WELCOME :3"

    private void Awake()
    {
        // Khởi tạo Singleton để các script khác dễ gọi
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Hàm để thay đổi dòng chữ trên bảng
    public void UpdateBoard(string message)
    {
        if (boardText != null)
        {
            boardText.text = message;
        }
    }

    // Hàm trung chuyển lệnh mua hàng sang cho PlayerStats
    public bool TryBuyItem(ItemData data)
    {
        if (PlayerStats.instance != null)
        {
            return PlayerStats.instance.TryBuyItem(data);
        }
        else
        {
            Debug.LogError("Lỗi: Không tìm thấy PlayerStats!");
            return false;
        }
    }
}