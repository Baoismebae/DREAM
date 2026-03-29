using UnityEngine;

public class SokobanCrateDiscrete : MonoBehaviour
{
    // Layer chứa Tường và các Thùng khác
    public LayerMask solidLayer;

    // Hàm này được gọi bởi Player khi đẩy. Trả về true nếu đẩy thành công 1 ô.
    public bool TryPushDiscrete(Vector2 direction)
    {
        // 1. Tính toán vị trí đích của THÙNG (cách vị trí hiện tại 1 ô)
        Vector2 targetPos = (Vector2)transform.position + direction;

        // 2. Kiểm tra xem ô phía sau thùng có vật cản (Tường/Thùng khác) không
        // Dùng OverlapCircle rất nhỏ ở ngay tâm ô đích
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.1f, solidLayer);

        if (hit == null) // Nếu ô tiếp theo trống
        {
            // DI CHUYỂN CHẴN: Nhảy ngay lập tức sang ô mới (không Lerp)
            transform.position = targetPos;

            // Kiểm tra xem có trúng đích không SAU KHI di chuyển
            CheckGoalDiscrete();

            // Phát tiếng đẩy thùng (gọi sang Audio Manager)
            if (SokobanAudioManager.Instance != null) SokobanAudioManager.Instance.PlaySFX(SokobanAudioManager.Instance.pushSound);

            return true; // Đẩy thành công
        }

        // Bị kẹt (có tường hoặc thùng khác phía sau)
        return false;
    }

    void CheckGoalDiscrete()
    {
        // Quét xem vị trí hiện tại của thùng có đè lên Goal nào không
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Goal"))
            {
                // Kích hoạt Goal chuyển xanh
                col.GetComponent<SokobanGoal>().ActivateGoal();

                // Báo cho Manager cộng điểm
                SokobanManager.Instance.AddFilledGoal();

                // Phát tiếng Ting!
                if (SokobanAudioManager.Instance != null) SokobanAudioManager.Instance.PlaySFX(SokobanAudioManager.Instance.goalSound);

                // Thùng biến mất
                Destroy(gameObject);
                break;
            }
        }
    }
}