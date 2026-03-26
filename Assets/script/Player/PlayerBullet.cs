using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public float damage = 20f;
    public GameObject hitParticlePrefab; 
    public float lifeTime = 3f;          

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Player")) return;

        
        ShootingAI rangedEnemy = hitInfo.GetComponentInParent<ShootingAI>();
        AI meleeEnemy = hitInfo.GetComponentInParent<AI>();
        BossAI boss = hitInfo.GetComponentInParent<BossAI>();
        SpringTrapAI springTrapEnemy = hitInfo.GetComponentInParent<SpringTrapAI>();

        bool hitEnemy = false;

        // Cắn máu quái
        if (boss != null) { boss.TakeDamage(damage); hitEnemy = true; }
        else if (rangedEnemy != null) { rangedEnemy.TakeDamage(damage); hitEnemy = true; }
        else if (meleeEnemy != null) { meleeEnemy.TakeDamage(damage); hitEnemy = true; }
        else if (springTrapEnemy != null) { springTrapEnemy.TakeDamage(damage); hitEnemy = true; }

        if (hitEnemy || !hitInfo.isTrigger) 
        {
            if (hitParticlePrefab != null)
            {
                Destroy(Instantiate(hitParticlePrefab, transform.position, Quaternion.identity), 0.33f);
            }
            Destroy(gameObject); // Hủy viên đạn
        }
    }
}
