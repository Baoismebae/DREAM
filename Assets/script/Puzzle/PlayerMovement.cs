using UnityEngine;

public class PlayerGridMovement : MonoBehaviour
{
    public float moveDistance = 1f;
    public LayerMask wallLayer;
    public LayerMask crateLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            TryMove(Vector2.up);

        if (Input.GetKeyDown(KeyCode.S))
            TryMove(Vector2.down);

        if (Input.GetKeyDown(KeyCode.A))
            TryMove(Vector2.left);

        if (Input.GetKeyDown(KeyCode.D))
            TryMove(Vector2.right);
    }

    void TryMove(Vector2 dir)
    {
        RaycastHit2D wallHit = Physics2D.Raycast(transform.position, dir, moveDistance, wallLayer);
        if (wallHit.collider != null)
            return;

        RaycastHit2D crateHit = Physics2D.Raycast(transform.position, dir, moveDistance, crateLayer);

        if (crateHit.collider != null)
        {
            Vector2 crateTarget = (Vector2)crateHit.collider.transform.position + dir;

            if (Physics2D.Raycast(crateHit.collider.transform.position, dir, moveDistance, wallLayer))
                return;

            crateHit.collider.transform.position = crateTarget;
        }

        transform.position += (Vector3)(dir * moveDistance);
    }
}