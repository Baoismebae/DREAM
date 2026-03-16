using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [Header("THÔNG SỐ TẤN CÔNG")]
    public float damage = 20f;         // Sát thương gây ra
    public float attackRange = 1.5f;   // Tầm xa của đòn đánh
    public float attackRate = 2f;      // Tốc độ đánh (số lần mỗi giây)
    private float nextAttackTime = 0f; // Thời gian chờ để đánh nhát tiếp theo

    [Header("KHÓA DI CHUYỂN")]
    public float attackDuration = 0.4f; // Thời gian đứng im khi vung kiếm
    private Playermovement movementScript; // Liên kết với script di chuyển

    [Header("CẤU HÌNH VA CHẠM")]
    public Transform attackPoint;      // Điểm phát ra đòn đánh
    public LayerMask enemyLayers;      // Chỉ định Layer nào là kẻ địch

    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
        
        // TỰ ĐỘNG TÌM VÀ KẾT NỐI VỚI SCRIPT DI CHUYỂN
        movementScript = GetComponent<Playermovement>(); 
        
        if (attackPoint == null) attackPoint = transform;
    }

    void Update()
    {
        HandleAttackPointPosition();

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
            {
                Attack(); 
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void HandleAttackPointPosition()
    {
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
        // GỌI TIẾN TRÌNH KHÓA DI CHUYỂN
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        // 1. BẬT KHÓA: Ép nhân vật đứng im
        if (movementScript != null) movementScript.isAttacking = true;

        // 2. Múa kiếm
        if (ani != null) ani.SetTrigger("Attack");

        // 3. Quét quái và trừ máu
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            ShootingAI rangedEnemy = enemy.GetComponentInParent<ShootingAI>();
            AI meleeEnemy = enemy.GetComponentInParent<AI>();
            BossAI boss = enemy.GetComponentInParent<BossAI>();

            if (boss != null)
            {
                boss.TakeDamage(damage); 
            }
            else if (rangedEnemy != null)
            {
                rangedEnemy.TakeDamage(damage); 
            }
            else if (meleeEnemy != null)
            {
                meleeEnemy.TakeDamage(damage); 
            }
            else
            {
                Health genericHealth = enemy.GetComponentInParent<Health>();
                if (genericHealth != null)
                {
                    genericHealth.TakeDamage(damage);
                }
            }
        }

        // 4. CHỜ MỘT LÚC (Ví dụ 0.4 giây) CHO ĐẾN KHI CHÉM XONG
        yield return new WaitForSeconds(attackDuration);

        // 5. TẮT KHÓA: Cho phép đi lại bình thường
        if (movementScript != null) movementScript.isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}