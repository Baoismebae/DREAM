using UnityEngine;

public class Bullet : MonoBehaviour
{
    GameObject target;
    public float speed;
    Rigidbody2D bulletRB;
    public float damage = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        if (target != null)
        {
            Vector2 moveDir = (target.transform.position - transform.position).normalized * speed;
            bulletRB.linearVelocity = new Vector2(moveDir.x, moveDir.y);
        }
        Destroy(this.gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage); 
            }
            Destroy(gameObject);
        }
        // (Tùy chọn) Hủy viên đạn nếu nó đập vào tường/vật cản
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
