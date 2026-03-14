using UnityEngine;

public class Coin : MonoBehaviour {
    public int value = 1; // Giá trị mỗi đồng xu
    [Header("Cài đặt Nam châm")]
    public float detectDistance = 3f; // Khoảng cách bắt đầu hút
    public float attractSpeed = 8f;  // Tốc độ bay về phía Player

    void Update()
    {
        // Nếu Mage tồn tại và khoảng cách đủ gần
        if (PlayerStats.instance != null)
        {
            float dist = Vector2.Distance(transform.position, PlayerStats.instance.transform.position);

            if (dist < detectDistance)
            {
                // Bay về phía Mage bằng MoveTowards
                transform.position = Vector2.MoveTowards(
                    transform.position, 
                    PlayerStats.instance.transform.position, 
                    attractSpeed * Time.deltaTime
                );
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            // Giả sử Player có script PlayerStats để giữ tiền
            collision.GetComponent<PlayerStats>().AddCoins(value);
            Destroy(gameObject); // Nhặt xong thì biến mất
        }
    }
}
