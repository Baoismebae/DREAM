using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [Header("DI CHUYỂN")]
    public float Speed = 5f;
    // --- ĐỔI THÀNH PUBLIC ĐỂ ITEM TĂNG TỐC CAN THIỆP ĐƯỢC ---
    [HideInInspector] public float currentSpeed;

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
    public float footstepDelay = 1f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        rb.freezeRotation = true;
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

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (onHorizontalStairs && movement.x != 0)
        {
            movement.y = movement.x * stairSlope;
        }

        if (movement.x != 0 || movement.y != 0)
        {
            ani.SetFloat("Horizontal", movement.x);
            ani.SetFloat("Vertical", movement.y);
        }
        ani.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x > 0)
        {
            sr.flipX = false;
        }
        else if (movement.x < 0)
        {
            sr.flipX = true;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("Tọa độ X khi vừa đổi hướng: " + transform.position.x);
        }

        bool isMoving = (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0);

        if (isMoving)
        {
            footstepTimer -= Time.deltaTime;
            if (footstepTimer <= 0f)
            {
                if (GlobalAudioManager.Instance != null) GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.footstep);
                footstepTimer = footstepDelay;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }

    void LateUpdate()
    {
        sr.sortingOrder = (int)(-transform.position.y * 100);
    }

    void FixedUpdate()
    {
        if (isAttacking) return;

        if (movement.sqrMagnitude > 0.01f)
        {
            rb.linearVelocity = movement.normalized * currentSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Stairs"))
        {
            onHorizontalStairs = true;

            Stair currentStair = collision.GetComponent<Stair>();
            if (currentStair != null)
            {
                stairSlope = currentStair.slope;
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