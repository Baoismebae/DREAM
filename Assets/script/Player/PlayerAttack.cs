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
        // 1. Kích hoạt Animation
        if (ani != null) ani.SetTrigger("Attack");

        // 2. Tạo một vòng tròn ảo để kiểm tra va chạm với quái
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // 3. Gây sát thương lên từng quái trúng đòn
        foreach (Collider2D enemy in hitEnemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log("Đã chém " + enemy.name + " mất " + damage + " máu!");
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