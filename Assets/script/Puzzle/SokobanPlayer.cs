using UnityEngine;
using UnityEngine.SceneManagement;

public class SokobanPlayer : MonoBehaviour
{
    [Header("Cấu hình Layer")]
    public LayerMask wallLayer;
    public LayerMask boxLayer;

    [Header("Cấu hình di chuyển")]
    public float moveUnit = 1f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetLevel();
        }

        Vector2 dir = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) dir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) dir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) dir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) dir = Vector2.right;

        if (dir != Vector2.zero)
        {
            TryMove(dir);
        }
    }

    // CHỈ GIỮ LẠI MỘT HÀM TRYMOVE NÀY
    void TryMove(Vector2 direction)
    {
        // Sử dụng Vector2 để triệt tiêu sai số trục Z, tránh lệch Y
        Vector2 currentPos = transform.position;
        Vector2 targetPos = currentPos + direction * moveUnit;

        // 1. Kiểm tra Tường
        if (Physics2D.OverlapCircle(targetPos, 0.2f, wallLayer))
        {
            return;
        }

        // 2. Kiểm tra Thùng
        Collider2D boxHit = Physics2D.OverlapCircle(targetPos, 0.2f, boxLayer);
        if (boxHit != null)
        {
            Vector2 boxTargetPos = targetPos + direction * moveUnit;

            // Kiểm tra vật cản sau thùng
            if (Physics2D.OverlapCircle(boxTargetPos, 0.2f, wallLayer | boxLayer))
            {
                return;
            }

            // Di chuyển thùng và khóa trục Z = 0 để không bị lệch
            boxHit.transform.position = new Vector3(boxTargetPos.x, boxTargetPos.y, 0);
        }

        // 3. Di chuyển Player và khóa trục Z = 0
        transform.position = new Vector3(targetPos.x, targetPos.y, 0);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
        Debug.Log("Đã tải lại màn chơi!");
    }
}