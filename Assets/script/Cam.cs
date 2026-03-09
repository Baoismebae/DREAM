using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform player;     // kéo player vào đây trong Inspector
    public Vector3 offset;       // khoảng cách giữa camera và player
    public float smoothSpeed;  // độ mượt khi camera di chuyển
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    // Nếu ô Player đang trống (do mình vừa xóa nhân vật ở Map 2)
    if (player == null)
    {
        // Tự động tìm con Mage chính thông qua Tag
        GameObject mage = GameObject.FindGameObjectWithTag("Player");
        if (mage != null)
        {
            player = mage.transform; // Gán lại mục tiêu cho AI
        }
    }    
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
