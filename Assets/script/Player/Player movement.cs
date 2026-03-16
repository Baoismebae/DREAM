using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float Speed = 5f;

    private Rigidbody2D rb;
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

        if (movement.x > 0)
            sr.flipX = false;
        else if (movement.x < 0)
            sr.flipX = true;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * Speed;
    }

    void LateUpdate()
    {
        sr.sortingOrder = (int)(-transform.position.y * 1000);
    }
}