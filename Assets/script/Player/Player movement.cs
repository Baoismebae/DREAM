using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [Header("DI CHUYỂN")]
    public float Speed = 5f;
    private float currentSpeed;

    [Header("CẦU THANG NGANG")]
    [HideInInspector]
    public bool onHorizontalStairs = false;
    public float stairSlope = -0.4f;

    [Header("THÀNH PHẦN (COMPONENTS)")]
    public Rigidbody2D rb;
    public Animator ani;
    private SpriteRenderer sr;

    [Header("TRẠNG THÁI")]
    public bool isAttacking = false;

    private Vector2 movement;

    private float footstepTimer = 0f;
    public float footstepDelay = 1f; // Tốc độ phát tiếng bước chân (chỉnh cho khớp với animation)

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        // QUAN TRỌNG: Khóa xoay để tránh vật lý làm nhân vật quay tròn
        rb.freezeRotation = true;
        // Đảm bảo không bị trọng lực kéo xuống trong game Top-down
        rb.gravityScale = 0f;

        currentSpeed = Speed;
    }

    void Update()
    {
        if (isAttacking)
        {
            movement = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 1. Lấy Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 2. Xử lý cầu thang
        if (onHorizontalStairs && movement.x != 0)
        {
            movement.y = movement.x * stairSlope;
        }

        // 3. Animation (Đã xóa bỏ đoạn trùng lặp)
        if (movement.x != 0 || movement.y != 0)
        {
            ani.SetFloat("Horizontal", movement.x);
            ani.SetFloat("Vertical", movement.y);
        }
        ani.SetFloat("Speed", movement.sqrMagnitude);

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
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
        Debug.Log("Tọa độ X khi vừa đổi hướng: " + transform.position.x);
        }

        // Giả sử biến di chuyển của bạn là moveX, moveY hoặc rb.velocity.magnitude
        bool isMoving = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0);

        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                if (GlobalAudioManager.Instance != null) GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.footstep);
                footstepTimer = footstepDelay; // Reset đồng hồ
            }
        }
        else
        {
            footstepTimer = 0f; // Dừng lại là reset ngay để bước tiếp theo kêu luôn
        }
    }

    void LateUpdate()
    {
        // Sắp xếp lớp hiển thị
        sr.sortingOrder = (int)(-transform.position.y * 100);
    }

    void FixedUpdate()
    {
        if (isAttacking) return;

        if (movement.sqrMagnitude > 0.01f)
        {
            // Dùng gán trực tiếp để tránh bị cộng dồn lực gây teleport/văng xa
            rb.linearVelocity = movement.normalized * currentSpeed;
        }
        else
        {
            // Dừng hẳn để không bị trôi (Drifting)
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            onHorizontalStairs = true;
            
            // 🌟 ĐIỂM MẤU CHỐT: Lấy độ dốc từ chính cái cầu thang đang dẫm lên
            Stair currentStair = collision.GetComponent<Stair>();
            if (currentStair != null)
            {
                stairSlope = currentStair.slope; // Cập nhật độ dốc mới cho Mage
            }

            currentSpeed = Speed * 0.8f; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            onHorizontalStairs = false;
            currentSpeed = Speed;
        }
    }
}