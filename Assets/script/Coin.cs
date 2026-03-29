using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    [Header("Cài đặt Nam châm")]
    public float detectDistance = 3f;
    public float attractSpeed = 0.25f; // Dùng cho Lerp (tầm 0.1 đến 0.5)

    // Thêm biến cờ này để chống lỗi 1 đồng vàng bị ăn 2 lần
    private bool isCollected = false;

    void Update()
    {
        // Chỉ hút khi chưa bị ăn
        if (PlayerStats.instance != null && !isCollected)
        {
            // 1. Tính khoảng cách thực tế (float)
            float dist = Vector2.Distance(transform.position, PlayerStats.instance.transform.position);

            // 2. Nếu nằm trong tầm hút
            if (dist < detectDistance)
            {
                // Di chuyển bằng Lerp
                transform.position = Vector2.Lerp(transform.position, PlayerStats.instance.transform.position, attractSpeed);

                // 3. Nếu cực gần thì nhặt luôn
                if (dist < 0.3f)
                {
                    CollectCoin();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Chỉ nhặt khi chạm Player và chưa bị ăn
        if (collision.CompareTag("Player") && !isCollected)
        {
            CollectCoin();
        }
    }

    // GỘP CHUNG LOGIC NHẶT VÀNG VÀO ĐÂY CHO GỌN
    private void CollectCoin()
    {
        // Khóa lại ngay lập tức, các lệnh Update hay Trigger khác không gọi được nữa
        isCollected = true;

        // 1. Cộng tiền
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.AddCoins(coinValue);
        }

        // 2. PHÁT ÂM THANH LỤM VÀNG (Keng!)
        if (GlobalAudioManager.Instance != null)
        {
            GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.coinPickup);
        }

        // 3. Hủy đồng vàng
        Destroy(gameObject);
    }
}