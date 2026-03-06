using UnityEngine;

public class Cam : MonoBehaviour
{
    public Transform player;     // kéo player vào đây trong Inspector
    public Vector3 offset;       // khoảng cách giữa camera và player
    public float smoothSpeed;  // độ mượt khi camera di chuyển
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
