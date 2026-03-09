using UnityEngine;

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
    public float detectionRange = 5f;
    public float attackCooldown = 1.5f; // Thời gian nghỉ giữa 2 nhát chém
    public Transform player;
    public LayerMask obstacleLayer;

    private Vector2 startPosition; 
    private Vector2 nextPoint;
    private float waitCounter;
    private bool isWaiting = false;
    private bool isChasing = false;
    private bool isAttacking = false; // Biến khóa trạng thái tấn công
    private Animator anim;

    void Start()
    {
       anim = GetComponent<Animator>();
        startPosition = transform.position;
        
        isWaiting = false; 
        SetNewPatrolPoint(); 
        
        waitCounter = Random.Range(minWaitTime, maxWaitTime);
    }

    // Update is called once per frame
    void Update()
    {
        // ==========================================
        // CHỐT KHÓA TRẠNG THÁI: NGĂN TRƯỢT BĂNG KHI CHÉM
        // ==========================================
        if (isAttacking)
        {
            // Quái bị ép đứng im, không được chạy lệnh Chase hay Patrol bên dưới.
            // Nhưng ta vẫn cho phép nó lật mặt (Flip) quay theo Player để đòn chém không bị đánh vào không khí.
            float scaleX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(player.position.x > transform.position.x ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);
            
            return; // Lệnh này bắt Unity dừng đọc code tại đây và thoát hàm Update
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // --- HỆ THỐNG TRẠNG THÁI BÌNH THƯỜNG ---

        // 1. Đã vào sát Player -> Đứng lại và Chém
        if (distanceToPlayer <= stopDistance)
        {
            isChasing = false;
            isWaiting = true; // Chuyển sang đứng yên
            AttackLogic();
        }
        // 2. Trong tầm nhìn nhưng chưa đủ gần -> Đuổi theo
        else if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            isWaiting = false;
            ChasePlayer(distanceToPlayer);
        }
        // 3. Ngoài tầm nhìn -> Về trạng thái tuần tra
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

    // ================= LOGIC TẤN CÔNG =================
    void AttackLogic()
    {
        // Luôn quay mặt về phía Player khi đang chém
        float scaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(player.position.x > transform.position.x ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);

        // Nếu không vướng đòn đánh cũ thì tung đòn mới
        if (!isAttacking)
        {
            StartCoroutine(AttackSequence());
        }
    }

    System.Collections.IEnumerator AttackSequence()
    {
        isAttacking = true;

        // Kích hoạt Animation chém
        if (anim != null) anim.SetTrigger("MeleeAttack");

        // Chờ quái chém xong và nghỉ ngơi (Cooldown)
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false; // Mở khóa để chém nhát tiếp theo
    }

    // ================= LOGIC DI CHUYỂN & TUẦN TRA (Giữ nguyên của bạn) =================
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

    void SetNewPatrolPoint()
    {
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

    bool IsPointInPolygon(Vector2[] poly, Vector2 p) {
        bool inside = false;
        for (int i = 0, j = poly.Length - 1; i < poly.Length; j = i++) {
            if (((poly[i].y > p.y) != (poly[j].y > p.y)) &&
                (p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)) {
                inside = !inside;
            }
        }
        return inside;
    }

    void UpdateAnimation()
    {
        if (anim == null) return;

        // Quái chỉ hiện anim chạy khi không bị Wait và không bị khóa bởi trạng thái Attacking
        bool isMoving = !isWaiting && !isAttacking;
        
        if (isChasing && Vector2.Distance(transform.position, player.position) <= stopDistance)
        {
            isMoving = false;
        }

        anim.SetBool("isRunning", isMoving);
    }

    private void OnDrawGizmosSelected()
    {
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
        Gizmos.DrawWireSphere(transform.position, stopDistance); // Thêm vòng tròn báo tầm chém
    }
}

