using UnityEngine;

public class SokobanPlayerDiscrete : MonoBehaviour
{
    // Layer chứa Tường và các Thùng khác
    public LayerMask solidLayer;

    void Update()
    {
        // Mỗi lần bấm nút chỉ thực hiện di chuyển 1 lần chẵn (dùng GetKeyDown)
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) TryMoveDiscrete(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) TryMoveDiscrete(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) TryMoveDiscrete(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) TryMoveDiscrete(Vector2.right);
    }

    void TryMoveDiscrete(Vector2 direction)
    {
        // 1. Tính toán vị trí đích của PLAYER (cách vị trí hiện tại 1 ô)
        Vector2 targetPos = (Vector2)transform.position + direction;

        // 2. Kiểm tra va chạm ở ô đích
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.1f, solidLayer);

        if (hit == null)
        {
            // ĐƯỜNG TRỐNG -> Nhảy ngay lập tức sang ô mới
            transform.position = targetPos;

            // Phát tiếng bước đi
            if (SokobanAudioManager.Instance != null) SokobanAudioManager.Instance.PlaySFX(SokobanAudioManager.Instance.moveSound);
        }
        else if (hit.CompareTag("Crate"))
        {
            // ĐỤNG THÙNG -> Gọi hàm đẩy (Hàm mới ở trên)
            // LƯU Ý: Phải get đúng component mới: SokobanCrateDiscrete
            SokobanCrateDiscrete crate = hit.GetComponent<SokobanCrateDiscrete>();

            if (crate != null && crate.TryPushDiscrete(direction))
            {
                // Thùng đẩy thành công -> Player cũng nhảy theo 1 ô
                transform.position = targetPos;
                // Tiếng đẩy thùng đã được phát bên trong script Crate
            }
        }
    }
}