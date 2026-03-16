using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public LayerMask crateLayer;

    void TryPush(Vector2 dir)
    {
        Vector2 pos = transform.position + (Vector3)dir;

        Collider2D crate = Physics2D.OverlapCircle(pos, 0.2f, crateLayer);

        if (crate != null)
        {
            crate.transform.position += (Vector3)dir;
        }
    }
}