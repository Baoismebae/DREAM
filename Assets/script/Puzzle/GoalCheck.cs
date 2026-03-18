using UnityEngine;

public class GoalCheck : MonoBehaviour
{
    private bool isFinished = false;
    private SpriteRenderer sr;
    private SokobanLogic logic;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        logic = FindObjectOfType<SokobanLogic>();
    }

    void Update()
    {
        // Nếu ô này đã xong thì không cần check nữa
        if (isFinished) return;

        // Tìm xem có cái thùng (Box) nào đang nằm đè lên mình không
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 0.1f);

        if (hit != null && hit.CompareTag("box"))
        {
            ConfirmGoal(hit.gameObject);
        }
    }

    void ConfirmGoal(GameObject box)
    {
        isFinished = true;

        // 1. Hiệu ứng: Thùng biến mất, Túi đổi màu
        box.SetActive(false);
        sr.color = logic.winColor;

        // 2. Báo cáo về đầu não
        logic.GoalOccupied();
    }
}