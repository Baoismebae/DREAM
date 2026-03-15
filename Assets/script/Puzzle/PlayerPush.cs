using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float pushPower = 2f;

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("stone"))
        {
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            Vector2 dir = col.transform.position - transform.position;
            rb.AddForce(dir * pushPower);
        }
    }
}