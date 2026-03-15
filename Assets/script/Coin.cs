using UnityEngine;

public class Coin : MonoBehaviour {
    public int coinValue = 1; 
    [Header("Cài đặt Nam châm")]
    public float detectDistance = 3f; 
    public float attractSpeed = 0.25f; // Dùng cho Lerp (tầm 0.1 đến 0.5)

    void Update()
    {
        if (PlayerStats.instance != null)
        {
            // 1. Tính khoảng cách thực tế (float)
            float dist = Vector2.Distance(transform.position, PlayerStats.instance.transform.position);

            // 2. Nếu nằm trong tầm hút
            if (dist < detectDistance)
            {
                // Di chuyển bằng Lerp (trả về Vector2 để gán vào position)
                transform.position = Vector2.Lerp(transform.position, PlayerStats.instance.transform.position, attractSpeed);

                // 3. Nếu cực gần thì nhặt luôn
                if (dist < 0.3f) 
                {
                    PlayerStats.instance.AddCoins(coinValue);
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            PlayerStats stats = collision.GetComponent<PlayerStats>();
            if (stats != null) {
                stats.AddCoins(coinValue); // Đổi thành coinValue cho khớp
                Destroy(gameObject); 
            }
        }
    }
}