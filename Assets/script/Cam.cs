using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform target;     
    public Vector3 offset = new Vector3(0, 5.3f, -10f); // Lấy theo thông số cũ của cậu
    public float smoothSpeed = 0.125f;

    [Header("Giới hạn di chuyển (Boundaries)")]
    public float minX, maxX;
    public float minY, maxY;

    void Start()
    {
        // Tự tìm Player nếu chưa kéo vào
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }
    }

    void LateUpdate() // Dùng LateUpdate để mượt hơn khi nhân vật di chuyển
    {
        if (target == null) return;

        // 1. Tính toán vị trí mong muốn (có Offset)
        Vector3 desiredPosition = target.position + offset;

        // 2. Làm mượt chuyển động
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 3. Khóa vị trí trong biên (Chỉ khóa X và Y, giữ nguyên Z của Camera)
        float clampedX = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}