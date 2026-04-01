using UnityEngine;

public class Backgroundcontroller : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        // 🌟 NÂNG CẤP 1: Tự động tìm Camera nếu bạn quên kéo thả (Rất tiện khi qua Map mới)
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    // 🌟 NÂNG CẤP 2: Đổi thành LateUpdate để mượt mà 100% với Cinemachine
    void LateUpdate()
    {
        if (cam == null) return; // Chống lỗi văng game nếu lỡ mất Camera

        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        // Logic lặp nền vô tận của bạn
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}