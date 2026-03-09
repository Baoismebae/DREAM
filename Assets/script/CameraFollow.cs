using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Để trống ở Map 2, code sẽ tự tìm con Mage cho cậu
    public float smoothing = 0.125f; 

    public float minX, maxX;
    public float minY, maxY;

    // THÊM PHẦN NÀY: Tự động tìm Player khi Map mới load xong
    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

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