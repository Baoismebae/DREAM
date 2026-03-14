using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public bool onHorizontalStairs = false; // Có đang đứng trên cầu thang ngang không?
    public float stairSlope = 0.5f; //độ dốc
    public float Speed;
    public Rigidbody2D rb;
    private SpriteRenderer sr;
    public bool isAttacking = false;
    public Animator ani;

    Vector2 movement;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. KHÓA TẠI ĐÂY: Xóa sạch hướng đi cũ và phanh gấp
        if (isAttacking)
        {
            movement = Vector2.zero; 
            rb.linearVelocity = Vector2.zero; 
            return; 
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (onHorizontalStairs)
        {
            if (movement.x != 0) 
            {
                // Bấm đi ngang -> Tự động nội suy thêm trục Y để đi xéo theo dốc
                movement.y = movement.x * stairSlope;
            }
            else 
            {
                // Không bấm đi ngang -> Khóa trục Y, không cho bấm Lên/Xuống để ăn gian
                movement.y = 0;
            }
        }

        ani.SetFloat("Horizontal", movement.x);
        ani.SetFloat("Vertical", movement.y);
        ani.SetFloat("Speed", movement.magnitude);

        if (movement.x > 0) sr.flipX = false;
        else if (movement.x < 0) sr.flipX = true;
    }

    void LateUpdate()
    {
        sr.sortingLayerName = "Default";
        sr.sortingOrder = (int)(-transform.position.y * 1000);
    }

    void FixedUpdate()
    {
        // 2. KHÓA TẠI ĐÂY NỮA: Chặn vật lý không cho đẩy nhân vật đi
        if (isAttacking) return; 

        if (movement.sqrMagnitude > 0.01f) 
        {
            rb.linearVelocity = movement.normalized * Speed;
        }
        else 
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.2f); 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            onHorizontalStairs = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            onHorizontalStairs = false;
        }
    }
}