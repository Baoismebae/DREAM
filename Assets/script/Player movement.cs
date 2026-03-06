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
    }

    void Update()
    {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            ani.SetFloat("Horizontal", movement.x);
            ani.SetFloat("Vertical", movement.y);
            ani.SetFloat("Speed", movement.sqrMagnitude);

        // Lật nhân vật khi đi trái phải
        if (movement.x > 0)
        {
            sr.flipX = false;
        }
        else if (movement.x < 0)
        {
            sr.flipX = true;
        }

        // Sắp xếp layer theo trục Y 
        sr.sortingOrder = (int)(-transform.position.y * 100);
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * Speed;
    }
}
