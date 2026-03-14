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
    
    // ĐÃ SỬA CHỮ 'm' THƯỜNG Ở ĐÂY:
    private Playermovement movementScript; 

    [Header("CẤU HÌNH VA CHẠM")]
    public Transform attackPoint;      // Điểm phát ra đòn đánh
    public LayerMask enemyLayers;      // Chỉ định Layer nào là kẻ địch

    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
        
        // ĐÃ SỬA CHỮ 'm' THƯỜNG Ở ĐÂY:
        movementScript = GetComponent<Playermovement>(); 
        
        if (attackPoint == null) attackPoint = transform;
    }

    void Update()
    {
        HandleAttackPointPosition();

        // Kiểm tra phím bấm và thời gian hồi chiêu
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
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        // 1. KHÓA DI CHUYỂN
        if (movementScript != null) movementScript.isAttacking = true;

        // 2. Kích hoạt Animation chém
        if (ani != null) ani.SetTrigger("Attack");

        // 3. Quét quái và xử lý sát thương
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
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
                Health genericHealth = enemy.GetComponentInParent<Health>();
                if (genericHealth != null)
                {
                    genericHealth.TakeDamage(damage);
                    Debug.Log("Đã chém trúng vật thể mất " + damage + " máu!");
                }
            }
        }

        // 4. CHỜ DIỄN XONG NHÁT CHÉM (Dừng lại 0.4 giây)
        yield return new WaitForSeconds(attackDuration);

        // 5. MỞ KHÓA DI CHUYỂN
        if (movementScript != null) movementScript.isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}