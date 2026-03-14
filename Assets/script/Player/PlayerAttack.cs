using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("THÔNG SỐ TẤN CÔNG")]
    public float damage = 20f;         // Sát thương gây ra
    public float attackRange = 1.5f;   // Tầm xa của đòn đánh
    public float attackRate = 2f;      // Tốc độ đánh (số lần mỗi giây)
    private float nextAttackTime = 0f; // Thời gian chờ để đánh nhát tiếp theo

    [Header("CẤU HÌNH VA CHẠM")]
    public Transform attackPoint;      // Điểm phát ra đòn đánh (thường đặt ở phía trước Mage)
    public LayerMask enemyLayers;      // Chỉ định Layer nào là kẻ địch

    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
        
        // Nếu quên chưa kéo attackPoint vào Inspector, hãy dùng chính vị trí Mage
        if (attackPoint == null) attackPoint = transform;
    }

    void Update()
    {
        HandleAttackPointPosition();

        // Kiểm tra phím bấm (ví dụ phím J hoặc Chuột trái) và thời gian hồi chiêu
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
            {
                Attack();
                // Tính toán thời gian cho lần đánh kế tiếp
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void HandleAttackPointPosition()
    {
        // Kiểm tra hướng di chuyển từ script di chuyển hoặc từ input
        float moveX = Input.GetAxisRaw("Horizontal");

        if (moveX > 0) // Nhìn sang phải
        {
            attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, 0);
        }
        else if (moveX < 0) // Nhìn sang trái
        {
            attackPoint.localPosition = new Vector3(-Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, 0);
        }
    }

    void Attack()
    {
        // 1. Kích hoạt Animation chém của Player
        if (ani != null) ani.SetTrigger("Attack");

        // 2. Tạo vòng tròn ảo để quét trúng quái
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // 3. Xử lý sát thương
        foreach (Collider2D enemy in hitEnemies)
        {
            // DÙNG GetComponentInParent: Bắt buộc dùng cái này để thanh kiếm tự mò lên Object cha 
            // tìm script AI (phòng khi bạn gắn Collider ở Object con chứa hình ảnh)
            
            ShootingAI rangedEnemy = enemy.GetComponentInParent<ShootingAI>();
            AI meleeEnemy = enemy.GetComponentInParent<AI>();
            BossAI boss = enemy.GetComponentInParent<BossAI>();

            if (boss != null)
            {
                boss.TakeDamage(damage); 
                Debug.Log("Đã chém trúng Boss mất " + damage + " máu!");
            }
            else if (rangedEnemy != null)
            {
                rangedEnemy.TakeDamage(damage); 
                Debug.Log("Đã chém quái tầm xa mất " + damage + " máu!");
            }
            else if (meleeEnemy != null)
            {
                meleeEnemy.TakeDamage(damage); 
                Debug.Log("Đã chém quái cận chiến mất " + damage + " máu!");
            }
            else
            {
                // Dòng này phòng hờ bạn chém trúng các vật thể phụ (như thùng gỗ, trụ đá...)
                // vẫn đang xài script Health cũ
                Health genericHealth = enemy.GetComponentInParent<Health>();
                if (genericHealth != null)
                {
                    genericHealth.TakeDamage(damage);
                    Debug.Log("Đã chém trúng vật thể mất " + damage + " máu!");
                }
            }
        }
    }

    // Vẽ vòng tròn đỏ trong Scene để cậu dễ căn chỉnh tầm đánh
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}