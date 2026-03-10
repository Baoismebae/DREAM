using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("THÔNG SỐ ĐẠN")]
    public float speed = 5f;
    public float damage = 10f; 

    private Rigidbody2D bulletRB;
    private GameObject target;

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
                playerHealth.TakeDamage(damage); // Truyền lượng sát thương vào
            }

            Destroy(gameObject);
        }
        
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}

