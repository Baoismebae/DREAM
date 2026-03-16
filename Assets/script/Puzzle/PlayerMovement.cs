using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private bool isMoving;
    private Vector3 targetPos;

    public LayerMask wallLayer;
    public LayerMask crateLayer;

    private SpriteRenderer sr;

    void Start()
    {
        targetPos = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isMoving) return;

        Vector2 dir = Vector2.zero;

        if (Input.GetKeyDown(KeyCode.W)) dir = Vector2.up;
        if (Input.GetKeyDown(KeyCode.S)) dir = Vector2.down;
        if (Input.GetKeyDown(KeyCode.A)) dir = Vector2.left;
        if (Input.GetKeyDown(KeyCode.D)) dir = Vector2.right;

        if (dir != Vector2.zero)
        {
            Move(dir);
        }

        if (dir.x > 0) sr.flipX = false;
        if (dir.x < 0) sr.flipX = true;
    }

    void Move(Vector2 dir)
    {
        Vector3 nextPos = transform.position + new Vector3(dir.x, dir.y, 0);

        if (Physics2D.OverlapCircle(nextPos, 0.2f, wallLayer))
            return;

        Collider2D crate = Physics2D.OverlapCircle(nextPos, 0.2f, crateLayer);

        if (crate != null)
        {
            Vector3 crateNext = nextPos + new Vector3(dir.x, dir.y, 0);

            if (Physics2D.OverlapCircle(crateNext, 0.2f, wallLayer) ||
                Physics2D.OverlapCircle(crateNext, 0.2f, crateLayer))
                return;

            crate.transform.position = crateNext;
        }

        targetPos = nextPos;
        StartCoroutine(SmoothMove());
    }

    System.Collections.IEnumerator SmoothMove()
    {
        isMoving = true;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                moveSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
}