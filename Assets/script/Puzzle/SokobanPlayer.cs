using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện bắt buộc để thực hiện Reset

public class SokobanPlayer : MonoBehaviour
{
    [Header("Cấu hình Layer")]
    public LayerMask wallLayer; // Gán layer Wall trong Inspector
    public LayerMask boxLayer;  // Gán layer Box trong Inspector

    [Header("Cấu hình di chuyển")]
    public float moveUnit = 1f; // Khoảng cách di chuyển đúng 1 ô

    void Update()
    {
        // 1. Lệnh Reset bằng phím tắt R
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }

        Vector2 dir = Vector2.zero;

        // 2. Nhận phím bấm di chuyển
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) dir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) dir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) dir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) dir = Vector2.right;

        if (dir != Vector2.zero)
        {
            TryMove(dir);
        }
    }

    void TryMove(Vector2 direction)
    {
        // Tính toán vị trí Player muốn tới
        Vector2 targetPos = (Vector2)transform.position + direction * moveUnit;

        // KIỂM TRA TƯỜNG
        if (Physics2D.OverlapCircle(targetPos, 0.2f, wallLayer))
        {
            return;
        }

        // KIỂM TRA THÙNG (BOX)
        Collider2D boxHit = Physics2D.OverlapCircle(targetPos, 0.2f, boxLayer);
        if (boxHit != null)
        {
            // Tính toán vị trí Thùng sẽ bị đẩy tới
            Vector2 boxTargetPos = targetPos + direction * moveUnit;

            // Kiểm tra phía sau thùng có bị kẹt bởi tường hoặc thùng khác không
            if (Physics2D.OverlapCircle(boxTargetPos, 0.2f, wallLayer | boxLayer))
            {
                return;
            }

            // Nếu phía sau trống, di chuyển thùng sang ô mới
            boxHit.transform.position = boxTargetPos;
        }

        // Sau khi kiểm tra xong xuôi, di chuyển Player
        transform.position = targetPos;
    }

    // 3. HÀM RESET: Dùng cho cả phím R và Nút bấm trên màn hình (UI Button)
    public void ResetLevel()
    {
        // Lấy Index của Scene hiện tại để load lại từ đầu
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        // Đảm bảo thời gian không bị ngưng đọng
        Time.timeScale = 1f;
        Debug.Log("Đã tải lại màn chơi!");
    }
}