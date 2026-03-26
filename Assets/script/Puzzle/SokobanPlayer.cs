using UnityEngine;
using UnityEngine.SceneManagement;

public class SokobanPlayer : MonoBehaviour
{
    [Header("Cấu hình Layer (Rất quan trọng)")]
    public LayerMask wallLayer;
    public LayerMask boxLayer;

    [Header("Cấu hình di chuyển")]
    public float moveUnit = 1f;

    [Header("Âm thanh (Kéo file mp3 vào đây)")]
    public AudioSource sfxSource;
    public AudioClip moveSound;
    public AudioClip pushSound;
    public AudioClip successSound;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ResetLevel();

        Vector2 dir = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) dir = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) dir = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) dir = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) dir = Vector2.right;

        if (dir != Vector2.zero) TryMove(dir);
    }

    void TryMove(Vector2 direction)
    {
        Vector2 targetPos = (Vector2)transform.position + direction * moveUnit;

        // 1. Kiểm tra Tường
        if (Physics2D.OverlapCircle(targetPos, 0.1f, wallLayer)) return;

        // 2. Kiểm tra Thùng
        Collider2D boxHit = Physics2D.OverlapCircle(targetPos, 0.1f, boxLayer);
        if (boxHit != null)
        {
            Vector2 boxTargetPos = targetPos + direction * moveUnit;

            // Kiểm tra vật cản sau thùng (Tường hoặc Thùng khác)
            if (Physics2D.OverlapCircle(boxTargetPos, 0.1f, wallLayer | boxLayer)) return;

            // ĐẨY ĐƯỢC: Di chuyển thùng trước
            boxHit.transform.position = new Vector3(boxTargetPos.x, boxTargetPos.y, 0);
            if (sfxSource && pushSound) sfxSource.PlayOneShot(pushSound);
        }
        else
        {
            // BƯỚC ĐI BÌNH THƯỜNG
            if (sfxSource && moveSound) sfxSource.PlayOneShot(moveSound);
        }

        // 3. Di chuyển Player
        transform.position = new Vector3(targetPos.x, targetPos.y, 0);
    }

    public void ResetLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}