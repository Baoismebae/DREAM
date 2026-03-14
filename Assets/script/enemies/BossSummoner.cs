using UnityEngine;

public class BossSummoner : MonoBehaviour
{
    [Header("ĐỐI TƯỢNG CẦN GỌI")]
    public GameObject bossObject; // Kéo con Boss đang tàng hình vào đây
    public GameObject interactUI; // (Tùy chọn) Chữ "Bấm E" hiện lên cho đẹp

    private bool isPlayerInRange = false; // Player có đang đứng gần không?
    private bool hasSummoned = false;     // Đã gọi Boss chưa? (Chống spam nút)

    void Start()
    {
        // Đảm bảo lúc mới vào game, mọi thứ đều tàng hình
        if (bossObject != null) bossObject.SetActive(false);
        if (interactUI != null) interactUI.SetActive(false);
    }

    void Update()
    {
        // Nếu Player đang đứng gần + Chưa gọi Boss + Bấm nút E
        if (isPlayerInRange && !hasSummoned && Input.GetKeyDown(KeyCode.E))
        {
            SummonBoss();
        }
    }

    void SummonBoss()
    {
        hasSummoned = true; // Khóa lại, không cho gọi 2 con Boss cùng lúc
        
        // Tắt chữ "Bấm E" đi
        if (interactUI != null) interactUI.SetActive(false);

        // BÙM! Bật Boss lên
        if (bossObject != null)
        {
            bossObject.SetActive(true);
            Debug.Log("Đã triệu hồi Boss thành công!");
            
            // Mẹo: Bạn có thể code thêm hiệu ứng rung màn hình, 
            // hoặc sinh ra Particle bùm chéo ở đây cho ngầu!
        }
    }

    // Khi Player bước vào vùng cảm ứng
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasSummoned)
        {
            isPlayerInRange = true;
            if (interactUI != null) interactUI.SetActive(true); // Hiện chữ "Bấm E"
        }
    }

    // Khi Player bước ra khỏi vùng cảm ứng
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactUI != null) interactUI.SetActive(false); // Ẩn chữ "Bấm E"
        }
    }
}
