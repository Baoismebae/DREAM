using UnityEngine;

public class bossBullet : MonoBehaviour
{
    public float damage = 15f; // Sát thương của 1 viên đạn Boss
    public float lifeTime = 5f; // Thời gian tồn tại trước khi tự biến mất

    void Start()
    {
        // Tự động xóa viên đạn sau 5 giây để không làm lag game nếu nó bay ra ngoài bản đồ
        Destroy(gameObject, lifeTime); 
    }

    // Khi viên đạn chạm vào vật gì đó
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Nếu chạm trúng Player
        if (hitInfo.CompareTag("Player"))
        {
            Health playerHealth = hitInfo.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            // Chạm Player xong thì viên đạn nổ/biến mất
            Destroy(gameObject);
        }
        // Tùy chọn: Nếu chạm vào tường (cần tag "Wall") thì cũng biến mất
        else if (hitInfo.CompareTag("Wall")) 
        {
            Destroy(gameObject);
        }
    }
}
