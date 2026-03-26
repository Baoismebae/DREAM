using UnityEngine;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
  [Header("CẤU HÌNH PHẠM VI ĐA GIÁC (POLYGON)")]
    public Vector2[] patrolPoints = new Vector2[] 
    {new Vector2(-2, -2), new Vector2(2, -2), new Vector2(2, 2), new Vector2(-2, 2)};

    public float moveSpeed = 2f;
    public float stopDistance = 0.8f;

    [Header("THỜI GIAN NGHỈ (WANDER TIME)")]
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    [Header("CHIẾN ĐẤU CẬN CHIẾN")]
    public float enemyDamage = 10f; 
    public float detectionRange = 5f;
    public float attackCooldown = 1.5f; 
    public Transform player;
    public LayerMask obstacleLayer;

    [Header("MÁU & BỊ THƯƠNG")]
    public float maxHealth = 50f;
    public GameObject coinPrefab; // Kéo Prefab đồng xu vào đây
    public int minCoins = 1;      // Số xu tối thiểu
    public int maxCoins = 3;      // Số xu tối đa
    private float currentHealth;
    private bool isDead = false;
    public Slider healthSlider; 
    private Vector3 healthBarScale; 

    private Vector2 startPosition; 
    private Vector2 nextPoint;
    private float waitCounter;
    private bool isWaiting = false;
    private bool isChasing = false;
    private bool isAttacking = false; 
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        
        // --- SETUP MÁU & THANH MÁU ---
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

        isWaiting = false; 
        SetNewPatrolPoint(); 
        waitCounter = Random.Range(minWaitTime, maxWaitTime);
    }

    void Update()
    {
        if (isDead || player == null) return;

        // Nếu đang trong quá trình chém -> KHÓA ĐỨNG IM
        if (isAttacking)
        {
            float scaleX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(player.position.x > transform.position.x ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);
            return; 
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 1. Gần sát Player -> Đứng lại & Chém
        if (distanceToPlayer <= stopDistance)
        {
            isChasing = false;
            isWaiting = true; 
            AttackLogic();
        }
        // 2. Thấy Player nhưng còn xa -> Đuổi theo
        else if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            isWaiting = false;
            ChasePlayer(distanceToPlayer);
        }
        // 3. Ngoài tầm nhìn -> Tuần tra
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

        UpdateAnimation();
    }

    // ================= LOGIC NHẬN SÁT THƯƠNG =================
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
        if (isDead) return; // Bảo vệ để quái không chết 2 lần
        isDead = true;

        if (anim != null) anim.SetBool("isDead", true);
        
        // --- LOGIC RƠI TIỀN ---
        if (coinPrefab != null)
        {
            int lootAmount = Random.Range(minCoins, maxCoins + 1);
            for (int i = 0; i < lootAmount; i++)
            {
                GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
                
                // Cho đồng xu văng ra ngẫu nhiên cho đẹp
                Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 scatter = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)).normalized;
                    rb.AddForce(scatter * 0.5f, ForceMode2D.Impulse);
                }
            }
        }
        // -----------------------

        GetComponent<Collider2D>().enabled = false;
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);
        Destroy(gameObject, 1.375f); 
    }

    // ================= LOGIC TẤN CÔNG =================
    void AttackLogic()
    {
        float scaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(player.position.x > transform.position.x ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);

        if (!isAttacking)
        {
            StartCoroutine(AttackSequence());
        }
    }

    System.Collections.IEnumerator AttackSequence()
    {
        isAttacking = true;
        
        // Gọi Trigger Attack
        if (anim != null) anim.SetTrigger("MeleeAttack");

        yield return new WaitForSeconds(0.3f);
        
        if (!isDead) 
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= stopDistance)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null) playerHealth.TakeDamage(enemyDamage); 
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false; 
    }

    // ================= LOGIC DI CHUYỂN =================
    void Patrol()
    {
        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                isWaiting = false;
                SetNewPatrolPoint();
            }
            return;
        }

        MoveTowards(nextPoint);

        if (Vector2.Distance(transform.position, nextPoint) < 0.2f)
        {
            isWaiting = true;
            waitCounter = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    void ChasePlayer(float currentDistance)
    {
        if (currentDistance > stopDistance)
        {
            MoveTowards(player.position);
        }
    }

    void MoveTowards(Vector2 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        if (target.x > transform.position.x + 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (target.x < transform.position.x - 0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void SetNewPatrolPoint() { /* Giữ nguyên của bạn */
        if (patrolPoints.Length < 3) return;

        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;
        foreach (Vector2 p in patrolPoints) {
            if (p.x < minX) minX = p.x; if (p.x > maxX) maxX = p.x;
            if (p.y < minY) minY = p.y; if (p.y > maxY) maxY = p.y;
        }

        for (int i = 0; i < 20; i++)
        {
            Vector2 randomPt = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            
            if (IsPointInPolygon(patrolPoints, randomPt))
            {
                Vector2 worldPt = startPosition + randomPt;
                Vector2 dir = (worldPt - (Vector2)transform.position).normalized;
                float dist = Vector2.Distance(transform.position, worldPt);
                
                if (!Physics2D.Raycast(transform.position, dir, dist, obstacleLayer))
                {
                    nextPoint = worldPt;
                    return;
                }
            }
        }
        nextPoint = startPosition;
    }
    bool IsPointInPolygon(Vector2[] poly, Vector2 p) { /* Giữ nguyên của bạn */
        bool inside = false;
        for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++) {
            if (((poly[i].y > p.y) != (poly[j].y > p.y)) &&
                (p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)) {
                inside = !inside;
            }
        }
        return inside;
    }

    // ================= XỬ LÝ ANIMATION (ĐÃ CẬP NHẬT) =================
    void UpdateAnimation()
    {
        if (anim == null) return;

        // Ưu tiên 1: Đang chờ đợi hoặc đang chuẩn bị chém -> Tắt hết di chuyển, về Idle
        if (isWaiting || isAttacking || (isChasing && Vector2.Distance(transform.position, player.position) <= stopDistance))
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", false);
            return; // Thoát ra, không xử lý gì thêm
        }

        // Ưu tiên 2: Đang rượt Player -> Tắt Walk, Bật Run
        if (isChasing)
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isRunning", true);
        }
        // Ưu tiên 3: Đi tuần tra thong thả -> Tắt Run, Bật Walk
        else 
        {
            anim.SetBool("isRunning", false);
            anim.SetBool("isWalking", true);
        }
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

    private void OnDrawGizmosSelected() { /* Giữ nguyên của bạn */
        Vector2 center = Application.isPlaying ? startPosition : (Vector2)transform.position;
        Gizmos.color = Color.green;
        if (patrolPoints != null && patrolPoints.Length > 1)
        {
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                Vector2 p1 = center + patrolPoints[i];
                Vector2 p2 = center + patrolPoints[(i + 1) % patrolPoints.Length];
                Gizmos.DrawLine(p1, p2);
            }
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}

