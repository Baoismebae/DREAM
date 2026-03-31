using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    [Header("UI Bảng Trắng")]
    public TextMeshProUGUI boardText;

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

    // Hàm mới dùng ItemData
    public void TryBuyItem(ItemData itemToBuy)
    {
        // Nhờ PlayerStats xử lý mua (trừ tiền, thêm đồ, phát âm thanh)
        bool success = PlayerStats.instance.TryBuyItem(itemToBuy);

        if (success)
        {
            UpdateBoard("You bought\n" + itemToBuy.itemName + " :)");
        }
        else
        {
            UpdateBoard("Not enough coin :(");
        }
    }
}