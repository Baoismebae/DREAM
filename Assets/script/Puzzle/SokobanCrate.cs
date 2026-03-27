using UnityEngine;

public class SokobanCrate : MonoBehaviour
{
    public LayerMask solidLayer; // Layer chứa Tường và các Thùng khác

    // Hàm này được gọi bởi Player khi đẩy
    public bool TryPush(Vector2 direction)
    {
        Vector2 targetPos = (Vector2)transform.position + direction;

        // Kiểm tra xem vị trí phía sau thùng có Tường hay Thùng khác không
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.1f, solidLayer);

        if (hit == null) // Nếu ô tiếp theo trống
        {
            transform.position = targetPos; // Di chuyển thùng
            CheckGoal(); // Kiểm tra xem có trúng đích không
            return true; // Báo cho Player là đã đẩy thành công
        }
        return false; // Báo cho Player là bị kẹt, không đẩy được
    }

    void CheckGoal()
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

                // Tiêu diệt thùng như bạn muốn
                Destroy(gameObject);
                break;
            }
        }
    }
}