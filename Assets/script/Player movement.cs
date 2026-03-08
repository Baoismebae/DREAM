using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    private SpriteRenderer sr;
    public Animator ani;

    Vector2 movement;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        ani.SetFloat("Horizontal", movement.x);
        ani.SetFloat("Vertical", movement.y);
        ani.SetFloat("Speed", movement.magnitude);

        // Lật nhân vật khi đi trái phải
        if (movement.x > 0)
        {
            sr.flipX = false;
        }
        else if (movement.x < 0)
        {
            sr.flipX = true;
        }

        // Sắp xếp layer theo trục Y (top-down game)
        sr.sortingOrder = (int)(-transform.position.y * 100);
    }

    void FixedUpdate()
{
    if (movement.sqrMagnitude > 0.01f) 
    {
        rb.linearVelocity = movement.normalized * Speed;
    }
    else 
    {
        // Thay vì gán = Vector2.zero ngay, hãy dùng Lerp để giảm tốc mượt hơn
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, 0.2f); 
    }
}
}