using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [Header("DI CHUYỂN")]
    public float Speed = 5f;
    private float currentSpeed; // Lưu tốc độ hiện tại để thay đổi khi leo dốc

    [Header("CẦU THANG NGANG")]
    [HideInInspector] // Giấu ô tick này đi cho gọn Inspector
    public bool onHorizontalStairs = false;
    public float stairSlope = -0.4f; // Độ nghiêng: Dương (dốc lên phải), Âm (dốc lên trái)

    [Header("THÀNH PHẦN (COMPONENTS)")]
    public Rigidbody2D rb;
    public Animator ani;
    private SpriteRenderer sr;

    [Header("TRẠNG THÁI")]
    public bool isAttacking = false; // Ổ khóa để PlayerAttack gọi sang

    private Vector2 movement;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        currentSpeed = Speed; // Mặc định vào game tốc độ thực = tốc độ gốc
    }

    void Update()
    {
        // 1. NẾU ĐANG CHÉM -> KHÓA ĐỨNG IM CỨNG NGẮC
        if (isAttacking)
        {
            movement = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            return; // Thoát luôn, không đọc lệnh đi lại nữa
        }

        // 2. LẤY NÚT BẤM DI CHUYỂN
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 3. XỬ LÝ LEO CẦU THANG NGANG (ĐI XÉO)
        if (onHorizontalStairs)
        {
            if (movement.x != 0)
            {
                // Tự động nội suy trục Y để đi xéo theo độ dốc
                movement.y = movement.x * stairSlope;
            }
            else
            {
                // Khóa trục Y, không cho đi thẳng Lên/Xuống ăn gian khi đang ở cầu thang
                movement.y = 0;
            }
        }

        if (movement.x != 0 || movement.y != 0)
        {
            ani.SetFloat("Horizontal", movement.x);
            ani.SetFloat("Vertical", movement.y);
        }

        if (movement.x != 0 || movement.y != 0)
        {
            ani.SetFloat("Horizontal", movement.x);
            ani.SetFloat("Vertical", movement.y);
        }

        // Riêng Speed thì vẫn phải gửi liên tục để Animator biết lúc nào nên đứng im
        ani.SetFloat("Speed", movement.magnitude);

        if (movement.x > 0)
        {
            sr.flipX = false; // Quay sang phải
        }
        else if (movement.x < 0)
        {
            sr.flipX = true;  // Quay sang trái
        }
    }

    void LateUpdate()
    {
        // 6. SẮP XẾP LỚP HIỂN THỊ (Người đứng dưới sẽ che người đứng trên)
        sr.sortingLayerName = "Default";
        sr.sortingOrder = (int)(-transform.position.y * 1000);
    }

    void FixedUpdate()
    {
        // 7. KHÓA VẬT LÝ KHI ĐANG CHÉM (Tránh bị trượt băng)
        if (isAttacking) return;

        // 8. ĐẨY LỰC DI CHUYỂN VÀ PHANH LẠI TỪ TỪ
        if (movement.sqrMagnitude > 0.01f)
        {
            rb.linearVelocity = movement.normalized * currentSpeed;
        }
        else
        {
            // Lerp giúp nhân vật giảm tốc từ từ nhìn tự nhiên hơn
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.2f);
        }
    }

    // 9. NHẬN DIỆN VÙNG CẦU THANG (Dùng Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            onHorizontalStairs = true;
            // (Tùy chọn): Giảm tốc độ đi một chút khi leo cầu thang cho chân thực
            currentSpeed = Speed * 0.8f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            onHorizontalStairs = false;
            // Trả lại tốc độ chạy bình thường khi xuống đất
            currentSpeed = Speed;
        }
    }
}