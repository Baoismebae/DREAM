using UnityEngine;

public class Playermovement : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D rb;
    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

        void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical   = Input.GetAxisRaw("Vertical");

        rb.linearVelocity = new Vector2(horizontal * Speed, vertical * Speed);
        sr.sortingOrder = (int)(-transform.position.y * 100);
    }
}
