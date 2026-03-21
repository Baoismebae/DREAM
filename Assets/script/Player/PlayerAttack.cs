using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
  [Header("CHẾ ĐỘ CHIẾN ĐẤU")]
    public bool isRangedMode = false; 
    public KeyCode switchWeaponKey = KeyCode.Q; 

    [Header("THÔNG SỐ CẬN CHIẾN")]
    public float damage = 20f;        
    public float attackRate = 2f;     
    private float nextAttackTime = 0f; 
    public Collider2D swordCollider; 
    public LayerMask enemyLayers; 
    public GameObject hitParticlePrefab; 

    [Header("QUYỀN TRƯỢNG & BẮN XA")]
    public GameObject bulletPrefab; 
    public Transform wandTransform;   
    public SpriteRenderer wandSprite; 
    public Transform firePoint;       
    public float bulletSpeed = 15f; 

    [Header("THỂ LỰC (STAMINA)")]
    public float maxStamina = 100f;
    public float staminaCostPerShot = 20f; 
    public float staminaRegenRate = 15f;   
    public Slider staminaSlider;           
    private float currentStamina;

    [Header("KHÓA DI CHUYỂN")]
    public float attackDuration = 0.4f; 
    private Playermovement movementScript; 
    private Animator ani;
    private SpriteRenderer playerSr; // Thêm biến để điều khiển lật ảnh Mage

    void Start()
    {
        ani = GetComponent<Animator>();
        movementScript = GetComponent<Playermovement>(); 
        playerSr = GetComponent<SpriteRenderer>(); // Lấy SpriteRenderer của Mage
        
        currentStamina = maxStamina;
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    void Update()
    {
        // Hồi thể lực
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            if (staminaSlider != null) staminaSlider.value = currentStamina;
        }

        // Đổi vũ khí
        if (Input.GetKeyDown(switchWeaponKey))
        {
            isRangedMode = !isRangedMode;
        }

        // Xoay gậy theo chuột
        HandleWandAiming();

        // Bắn / Chém
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
            {
                if (isRangedMode) 
                {
                    if (currentStamina >= staminaCostPerShot)
                    {
                        currentStamina -= staminaCostPerShot; 
                        if (staminaSlider != null) staminaSlider.value = currentStamina;
                        Shoot(); 
                        nextAttackTime = Time.time + 1f / attackRate;
                    }
                }
                else 
                {
                    Attack(); 
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        }
    }

    // ================= 🌟 ĐÃ LÀM LẠI: HÒA BÌNH VỚI PLAYERMOVEMENT =================
    void HandleWandAiming()
    {
        if (wandTransform == null) return;

        if (isRangedMode)
        {
            wandTransform.gameObject.SetActive(true); 

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            // 1. Lật mặt con Mage BẰNG FLIPX (Giống hệt cách Playermovement làm)
            if (playerSr != null)
            {
                if (mousePos.x > transform.position.x) playerSr.flipX = false;
                else if (mousePos.x < transform.position.x) playerSr.flipX = true;
            }

            // 2. Xoay Quyền trượng 360 độ (Toán học thuần túy, không sợ kẹt vì không có scale âm)
            Vector2 aimDirection = mousePos - wandTransform.position;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            
            // Trừ 90 độ vì ảnh gốc quyền trượng của cậu vẽ dựng đứng
            wandTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            // 3. Chống ngược Gậy khi xoay sang trái
            if (wandSprite != null)
            {
                // Vì gậy vẽ dựng đứng, nên khi xoay ngang ta dùng flipX để lật bề mặt của nó
                wandSprite.flipX = (mousePos.x < transform.position.x);
            }
        }
        else
        {
            wandTransform.gameObject.SetActive(false); 
        }
    }

    // ================= LOGIC ĐÁNH GẦN (Giữ nguyên) =================
    void Attack()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        if (movementScript != null) movementScript.isAttacking = true;
        if (ani != null) ani.SetTrigger("Attack");

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(enemyLayers);
        filter.useLayerMask = true;

        List<Collider2D> hitEnemies = new List<Collider2D>();
        if (swordCollider != null) Physics2D.OverlapCollider(swordCollider, filter, hitEnemies);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (hitParticlePrefab != null) Destroy(Instantiate(hitParticlePrefab, enemy.transform.position, Quaternion.identity), 0.2f);
        
            ShootingAI rangedEnemy = enemy.GetComponentInParent<ShootingAI>();
            AI meleeEnemy = enemy.GetComponentInParent<AI>();
            BossAI boss = enemy.GetComponentInParent<BossAI>();
            SpringTrapAI springTrapEnemy = enemy.GetComponentInParent<SpringTrapAI>();

            if (boss != null) boss.TakeDamage(damage); 
            else if (rangedEnemy != null) rangedEnemy.TakeDamage(damage); 
            else if (meleeEnemy != null) meleeEnemy.TakeDamage(damage); 
            else if (springTrapEnemy != null) springTrapEnemy.TakeDamage(damage); 
            else
            {
                Health genericHealth = enemy.GetComponentInParent<Health>();
                if (genericHealth != null) genericHealth.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(attackDuration);
        if (movementScript != null) movementScript.isAttacking = false;
    }

    // ================= LOGIC BẮN XA (Giữ nguyên) =================
    void Shoot()
    {
        if (ani != null) ani.SetTrigger("Attack"); 
        
        if (bulletPrefab != null && firePoint != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            Vector2 shootDirection = (mousePos - firePoint.position).normalized;

            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
            
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = shootDirection * bulletSpeed; 
            }
        }
    }
}