using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Kéo con player vào đây
    public float smoothing = 0.125f; // Độ mượt khi đuổi theo

    // Các con số cậu vừa đo ở Bước 1
    public float minX, maxX;
    public float minY, maxY;

    void LateUpdate()
{
    if (target != null)
    {
        // 1. Cam đuổi theo nhân vật như bình thường
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
        
        // 2. Làm mượt chuyển động của Cam trước
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPos, smoothing);
        
        // 3. CUỐI CÙNG: Khóa cứng vị trí đã được làm mượt đó vào trong biên
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);

        // Gán vị trí cuối cùng
        transform.position = smoothedPosition;
    }
}
}