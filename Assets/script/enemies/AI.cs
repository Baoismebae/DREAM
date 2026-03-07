using UnityEngine;

public class AI : MonoBehaviour
{
    [Header("CẤU HÌNH PHẠM VI HÌNH HỘP (BOX)")]
    [Tooltip("Chiều rộng (X) và Chiều cao (Y) của vùng tuần tra cố định")]
    public Vector2 patrolBoxSize;
    public float moveSpeed;
    public float stopDistance;

    [Header("THỜI GIAN NGHỈ (WANDER TIME)")]
    public float minWaitTime;
    public float maxWaitTime;

    [Header("CHIẾN ĐẤU")]
    public float detectionRange;
    public Transform player;
    public LayerMask obstacleLayer;

    // Biến điều khiển nội bộ
    private Vector2 startPosition; // Tâm cố định của hình hộp
    private Vector2 nextPoint;
    private float waitCounter;
    private bool isWaiting = false;
    private bool isChasing = false;


    //Anim cho nhân vật
     private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>(); // Lấy Animator gắn trên quái
    // ... giữ nguyên các dòng code Start cũ ...


        // Ghi nhớ vị trí đặt Enemy ban đầu làm tâm của hình hộp tuần tra
        startPosition = transform.position;
        
        // Bắt đầu tại chỗ và chuẩn bị chọn điểm tuần tra mới
        nextPoint = transform.position;
        waitCounter = Random.Range(minWaitTime, maxWaitTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // KIỂM TRA TRẠNG THÁI: Nếu thấy Player thì Chase, không thì Patrol
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            isWaiting = false; // Đang đuổi theo thì không đứng nghỉ
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            if (isChasing) // Vừa mất dấu Player, quay lại tuần tra
            {
                isChasing = false;
                isWaiting = true;
                waitCounter = minWaitTime;
                SetNewPatrolPoint(); 
            }
            Patrol();
        }

        UpdateAnimation();
    }

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
            return; // Dừng di chuyển khi đang nghỉ
        }

        MoveTowards(nextPoint);

        // Nếu đã đến gần điểm đích tuần tra
        if (Vector2.Distance(transform.position, nextPoint) < 0.2f)
        {
            isWaiting = true;
            waitCounter = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    void ChasePlayer(float currentDistance)
    {
        // Chỉ di chuyển nếu khoảng cách còn lớn hơn Stop Distance
        if (currentDistance > stopDistance)
        {
            MoveTowards(player.position);
        }
    }

    void MoveTowards(Vector2 target)
    {
        // Di chuyển nhân vật
        transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        // LẬT MẶT (FLIP): Dựa trên hướng di chuyển X
        if (target.x > transform.position.x + 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (target.x < transform.position.x - 0.01f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void SetNewPatrolPoint()
    {
        for (int i = 0; i < 10; i++) // Thử tìm điểm trống tối đa 10 lần
        {
            // Tính toán phạm vi hình hộp dựa trên startPosition
            float halfX = patrolBoxSize.x / 2f;
            float halfY = patrolBoxSize.y / 2f;

            float randomX = Random.Range(-halfX, halfX);
            float randomY = Random.Range(-halfY, halfY);

            Vector2 randomPoint = startPosition + new Vector2(randomX, randomY);

            // Kiểm tra vật cản (Raycast) để tránh chọn điểm nằm trong tường
            Vector2 direction = (randomPoint - (Vector2)transform.position).normalized;
            float distance = Vector2.Distance(transform.position, randomPoint);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleLayer);

            if (hit.collider == null)
            {
                nextPoint = randomPoint;
                return;
            }
        }
        // Nếu kẹt quá không tìm được điểm, quay về tâm cho an toàn
        nextPoint = startPosition; 
    }

    // Vẽ trực quan trong cửa sổ Scene
    private void OnDrawGizmosSelected()
    {
        // Tâm hình hộp: startPosition khi Play, transform.position khi thiết kế
        Vector3 center = Application.isPlaying ? (Vector3)startPosition : transform.position;

        // Vùng BOX tuần tra (Màu xanh lá)
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, new Vector3(patrolBoxSize.x, patrolBoxSize.y, 0));

        // Tầm phát hiện Player (Màu đỏ)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Điểm đích hiện tại (Màu vàng)
        if (!isChasing && Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, nextPoint);
            Gizmos.DrawSphere(nextPoint, 0.2f);
        }
    }
    void UpdateAnimation()
    {
    // Nếu đang chờ (Idle) hoặc đang đứng im ở Stop Distance
    if (isWaiting)
        {
        anim.SetBool("isRunning", false);
        }
    else if (isChasing)
        {
        float distance = Vector2.Distance(transform.position, player.position);
        // Nếu đang dí nhưng bị chặn bởi Stop Distance thì đứng yên
        anim.SetBool("isRunning", distance > stopDistance);
        }
    else
        {
        // Nếu không chờ, không dí đứng im thì chắc chắn đang đi tuần
        anim.SetBool("isRunning", true);
        }
    }
}

