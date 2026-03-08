using UnityEngine;

public class ShootingAI : MonoBehaviour
{
    [Header("CẤU HÌNH TUẦN TRA")]
    public Vector2[] patrolPoints = new Vector2[] { new Vector2(-2, -2), new Vector2(2, -2) };
    public float moveSpeed;
    public float stopDistance;

    [Header("CHIẾN ĐẤU")]
    public float detectionRange;
    public float shootingRange; 
    public float fireRate = 1f; 
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    public GameObject bullet;
    public GameObject bulletParent; 
    public Transform player;

    [Header("ANIMATION BẮN")]
    public float chargeTime = 0.5f; // Thời gian gồng trước khi đạn bay ra (giây)
    private bool isShootingAction = false; // Biến khóa: Đảm bảo quái không bị bắn chồng chéo

    private Vector2 startPosition; 
    private Vector2 nextPoint;
    private float waitCounter;
    private bool isWaiting = false;
    private bool isChasing = false;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        SetNewPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        // TRẠNG THÁI 1: Trong tầm bắn -> Đứng yên và Bắn
        if (distanceToPlayer <= shootingRange)
        {
            if (!isWaiting) 
            isChasing = false;
            isWaiting = true; 
            ShootLogic(); // Hàm này đã chứa Instantiate và Cooldown
        }
        // TRẠNG THÁI 2: Ngoài tầm bắn nhưng trong tầm nhìn -> Đuổi theo
        else if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            isWaiting = false;
            ChasePlayer(distanceToPlayer);
        }
        // TRẠNG THÁI 3: Ngoài tầm nhìn -> Đi tuần
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

    // --- CÁC HÀM NÀY PHẢI NẰM RIÊNG BIỆT ---

    void ShootLogic() 
    {
       // Lật mặt quái (Luôn nhìn theo Player dù đang đứng im)
        float scaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(player.position.x > transform.position.x ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);

        // Chỉ cho phép gọi chu trình bắn mới khi đã hoàn thành xong chu trình cũ (gồng + bắn + nghỉ)
        if (!isShootingAction) 
        {
            StartCoroutine(ShootSequence()); 
        }
    }

    // CHU TRÌNH: GỒNG -> BẮN -> NGHỈ IDLE
    System.Collections.IEnumerator ShootSequence()
    {
        // 1. KHÓA BÒNG LẶP: Đánh dấu là đang bận xử lý bắn
        isShootingAction = true;

        // 2. GỒNG (Phát Animation Tấn công)
        if (anim != null) anim.SetTrigger("Attack");

        // --- Đợi quái múa súng xong (Thời gian gồng) ---
        yield return new WaitForSeconds(chargeTime); 

        // 3. BẮN (Sinh ra viên đạn)
        if (bullet != null) 
        {
            Vector2 spawnPos = (bulletParent != null) ? (Vector2)bulletParent.transform.position : (Vector2)transform.position;
            Instantiate(bullet, spawnPos, Quaternion.identity);
        }

        // 4. NGHỈ NGƠI CHỜ COOLDOWN (Quay lại Idle 1 khoảng thời gian)
        // Lúc này biến fireRate sẽ đóng vai trò là "thời gian đứng chơi" sau khi bắn
        yield return new WaitForSeconds(fireRate);

        // 5. MỞ KHÓA: Cho phép quái gồng và bắn phát tiếp theo
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
        if (Vector2.Distance(transform.position, nextPoint) < 0.2f) 
        {
            isWaiting = true;
            waitCounter = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    void ChasePlayer(float d) 
    { 
        if (d > stopDistance) transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime); 
    }

    void SetNewPatrolPoint() 
    {
        if (patrolPoints.Length < 1) return;
        nextPoint = startPosition + patrolPoints[Random.Range(0, patrolPoints.Length)];
    }

    void UpdateAnimation() 
    {
        if (anim != null) anim.SetBool("isRunning", !isWaiting);
    }

    // Hàm này vẽ các đường hỗ trợ chỉ khi bạn Click chọn con Quái trong Hierarchy
    private void OnDrawGizmosSelected()
    {
        // 1. Xác định tâm của vùng tuần tra
        // Khi đang chạy game thì dùng startPosition, khi đang Edit thì dùng vị trí hiện tại
        Vector2 center = Application.isPlaying ? startPosition : (Vector2)transform.position;

        // 2. Vẽ đường nối các điểm Patrol (Màu Xanh Lá)
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                Vector2 p1 = center + patrolPoints[i];
                // Lấy điểm tiếp theo (nối điểm cuối về điểm đầu để tạo thành vòng kín)
                Vector2 p2 = center + patrolPoints[(i + 1) % patrolPoints.Length];

                Gizmos.DrawLine(p1, p2); // Vẽ đường nối
                Gizmos.DrawSphere(p1, 0.1f); // Vẽ nốt chấm tại mỗi điểm
            }
        }
        Gizmos.color = Color.cyan;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
    }
}
