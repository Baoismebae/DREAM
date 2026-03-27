using UnityEngine;

public class SokobanPlayer : MonoBehaviour
{
    public LayerMask solidLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) TryMove(Vector2.up);
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) TryMove(Vector2.down);
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) TryMove(Vector2.left);
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) TryMove(Vector2.right);
    }

    void TryMove(Vector2 direction)
    {
        Vector2 targetPos = (Vector2)transform.position + direction;

        // Dùng OverlapCircle để check ô phía trước có vật thể Solid nào không
        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.1f, solidLayer);

        if (hit == null)
        {
            // Trống -> đi tới
            transform.position = targetPos;
        }
        else if (hit.CompareTag("Crate"))
        {
            // Nếu là thùng -> gọi hàm đẩy của thùng
            SokobanCrate crate = hit.GetComponent<SokobanCrate>();
            if (crate != null && crate.TryPush(direction))
            {
                // Thùng đẩy được -> Player đi theo
                transform.position = targetPos;
            }
        }
    }
}