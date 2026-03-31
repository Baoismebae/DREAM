using UnityEngine;
using TMPro;

public class AutoCoinText : MonoBehaviour
{
    private TextMeshProUGUI myText;

    void Start()
    {
        // Tự động lấy component TextMeshPro gắn trên chính nó
        myText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Liên tục kiểm tra ví tiền của Player và tự động đổi chữ. 
        // Không cần ai phải ra lệnh hay gọi nó cả!
        if (PlayerStats.instance != null && myText != null)
        {
            myText.text = PlayerStats.instance.currentCoins.ToString() + " <sprite=0 voffset=0.1em>";
        }
    }
}