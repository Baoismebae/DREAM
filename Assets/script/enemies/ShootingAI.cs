using UnityEngine;
using UnityEngine.UI;

public class ShootingAI : MonoBehaviour
{
  [Header("CẤU HÌNH TUẦN TRA")]
    public Vector2[] patrolPoints = new Vector2[] { new Vector2(-2, -2), new Vector2(2, -2) };
    public float moveSpeed = 3f;
    public float stopDistance = 0.8f;

    [Header("CHIẾN ĐẤU")]
    public float shootingRange = 10f; 
    public float fireRate = 1.5f; // Thời gian đứng im (idle) chờ ở mỗi chu kỳ bắn
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    [Header("CHIẾN ĐẤU TẦM XA")]
    public float detectionRange = 15f;
    public GameObject bullet;
    public GameObject bulletParent; 
    public Transform player;

    [Header("MÁU & RỚT ĐỒ")]
    public UnityEngine.UI.Slider healthSlider;
    private Vector3 healthBarScale;
    public float maxHealth = 30f;
    
    // 🌟 ĐÃ THÊM: BIẾN CHỨA ĐỒNG XU
    public GameObject coinPrefab; 
    public int minCoins = 1;      
    public int maxCoins = 3;      

    private float currentHealth;
    private bool isDead = false;

    [Header("ANIMATION BẮN")]
    public float chargeTime = 1f; 
    private bool isShootingAction = false; 

    private Vector2 startPosition; 
    private Vector2 nextPoint;
    private float waitCounter;
    private bool isWaiting = false;
    private bool isChasing = false;
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; 
            healthSlider.value = currentHealth; 
            healthBarScale = healthSlider.transform.localScale; 
        }

        if (player == null)
        {
            GameObject targetObj = GameObject.FindGameObjectWithTag("Player");
            if (targetObj != null) player = targetObj.transform;
        }
        SetNewPatrolPoint();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange)
        {
            isChasing = false;
            isWaiting = true; 
            ShootLogic(); 
        }
        else if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            isWaiting = false;
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            if (isChasing) 
            { 
                isChasing = false; 
                isWaiting = false; 
                SetNewPatrolPoint(); 
            }
            Patrol();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;
        
        currentHealth -= damageAmount;
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (anim != null) anim.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        if (isDead) return; // Bảo vệ chống chết 2 lần giống SpringTrap
        isDead = true;

        if (anim != null) anim.SetBool("isDead", true);

        // ==========================================
        // 🌟 ĐÃ THÊM: LOGIC RƠI TIỀN KHI CHẾT
        // ==========================================
        if (coinPrefab != null)
        {
            int lootAmount = Random.Range(minCoins, maxCoins + 1);
            for (int i = 0; i < lootAmount; i++)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                
                // Cho đồng xu văng ra ngẫu nhiên
                Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 scatter = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)).normalized;
                    rb.AddForce(scatter * 0.5f, ForceMode2D.Impulse);
                }
            }
        }
        // ==========================================

        GetComponent<Collider2D>().enabled = false; // Tắt va chạm

        // Ẩn thanh máu đi cho đỡ vướng mắt
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        Destroy(gameObject, 1.375f); 
    }

    void ShootLogic() 
    {
        float scaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(player.position.x > transform.position.x ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);

        if (!isShootingAction) 
        {
            StartCoroutine(ShootSequence()); 
        }
    }

    System.Collections.IEnumerator ShootSequence()
    {
        isShootingAction = true;

        if (anim != null) anim.SetBool("isCharging", true);

        yield return new WaitForSeconds(chargeTime); 

        if (bullet != null && !isDead) 
        {
            Vector2 spawnPos = (bulletParent != null) ? (Vector2)bulletParent.transform.position : (Vector2)transform.position;
            Instantiate(bullet, spawnPos, Quaternion.identity);
        }

        if (anim != null) anim.SetBool("isCharging", false);

        yield return new WaitForSeconds(fireRate);

        isShootingAction = false;
    }

    void Patrol() 
    {
        if (isWaiting) 
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0) { isWaiting = false; SetNewPatrolPoint(); }
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, nextPoint, moveSpeed * Time.deltaTime);

        if (nextPoint.x > transform.position.x + 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (nextPoint.x < transform.position.x - 0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if (Vector2.Distance(transform.position, nextPoint) < 0.2f) 
        {
            isWaiting = true;
            waitCounter = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    void ChasePlayer(float d) 
    { 
        if (d > stopDistance) 
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime); 
            float scaleX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(player.position.x > transform.position.x ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    void SetNewPatrolPoint() 
    {
        if (patrolPoints.Length < 1) return;
        nextPoint = startPosition + patrolPoints[Random.Range(0, patrolPoints.Length)];
    }

    void LateUpdate()
    {
        if (healthSlider != null)
        {
            healthSlider.transform.localScale = new Vector3(
                healthBarScale.x * Mathf.Sign(transform.localScale.x),
                healthBarScale.y,
                healthBarScale.z
            );
        }
    }
}
