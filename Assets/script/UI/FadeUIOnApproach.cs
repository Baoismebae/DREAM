using UnityEngine;
using UnityEngine.UI;

public class FadeUIOnApproach : MonoBehaviour
{
    public float targetAlpha = 0.3f; // Độ mờ khi lại gần (0.3 là mờ vừa đủ đẹp)
    public float fadeSpeed = 8f;     // Tốc độ mờ nhanh hay chậm

    private Graphic[] uiElements;
    private Transform playerTransform;
    private RectTransform myRect;

    void Start()
    {
        uiElements = GetComponentsInChildren<Graphic>();
        myRect = GetComponent<RectTransform>();

        // Tự động quét bản đồ tìm nhân vật có mang thẻ "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    void Update()
    {
        // Nếu qua map mới mà mất Player thì tìm lại
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerTransform = player.transform;
            return;
        }

        // Dùng Camera chụp ảnh vị trí Player rồi in lên kính màn hình (Screen Space)
        Vector2 playerScreenPos = Camera.main.WorldToScreenPoint(playerTransform.position);

        // Kiểm tra xem vị trí đó có đang đâm vào khung InventoryBar không
        bool isOverlapping = RectTransformUtility.RectangleContainsScreenPoint(myRect, playerScreenPos, null);

        // Chốt số Alpha (Nếu chạm thì mờ, không thì sáng 100%)
        float currentTargetAlpha = isOverlapping ? targetAlpha : 1f;

        // Bắt đầu làm mờ mượt mà bằng Lerp
        foreach (var element in uiElements)
        {
            if (element != null)
            {
                Color c = element.color;
                c.a = Mathf.Lerp(c.a, currentTargetAlpha, Time.deltaTime * fadeSpeed);
                element.color = c;
            }
        }
    }
}