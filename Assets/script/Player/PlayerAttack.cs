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
    private SpriteRenderer playerSr; 

    void Start()
    {
        ani = GetComponent<Animator>();
        movementScript = GetComponent<Playermovement>(); 
        playerSr = GetComponent<SpriteRenderer>(); 
        
        currentStamina = maxStamina;
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    void Update()
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
            if (staminaSlider != null) staminaSlider.value = currentStamina;
        }

        if (Input.GetKeyDown(switchWeaponKey))
        {
            isRangedMode = !isRangedMode;
        }

        HandleWandAiming();

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

    void HandleWandAiming()
    {
        if (wandTransform == null) return;

        if (isRangedMode)
        {
            wandTransform.gameObject.SetActive(true); 

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            if (playerSr != null)
            {
                if (mousePos.x > transform.position.x) playerSr.flipX = false;
                else if (mousePos.x < transform.position.x) playerSr.flipX = true;
            }

            Vector2 aimDirection = mousePos - wandTransform.position;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            wandTransform.rotation = Quaternion.Euler(0, 0, angle - 90f);

            if (wandSprite != null) wandSprite.flipX = (mousePos.x < transform.position.x);
        }
        else
        {
            wandTransform.gameObject.SetActive(false); 
        }
    }

    void TriggerAttackAnimation()
    {
        if (ani == null) return;

        ani.SetBool("isRanged", isRangedMode);

        float finalAimX = 0f;
        float finalAimY = 0f;

        if (isRangedMode)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            Vector2 aimDir = (mousePos - transform.position).normalized;
            
            finalAimX = Mathf.Abs(aimDir.x); 
            finalAimY = aimDir.y;
        }
        else
        {
            float moveX = ani.GetFloat("Horizontal");
            float moveY = ani.GetFloat("Vertical");

            if (moveX == 0 && moveY == 0) moveX = 1f;

            Vector2 aimDir = new Vector2(moveX, moveY).normalized;
            finalAimX = Mathf.Abs(aimDir.x);
            finalAimY = aimDir.y;

            if (swordCollider != null && swordCollider.transform != this.transform)
            {
                if (playerSr != null && playerSr.flipX)
                {
                    swordCollider.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                }
                else
                {
                    swordCollider.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }

        ani.SetFloat("AimX", finalAimX);
        ani.SetFloat("AimY", finalAimY);
        ani.SetTrigger("Attack");
    }

    void Attack()
    {
        StartCoroutine(AttackRoutine());
    }

    IEnumerator AttackRoutine()
    {
        if (movementScript != null) movementScript.isAttacking = true;
        
        TriggerAttackAnimation(); 

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

    void Shoot()
    {
        TriggerAttackAnimation(); 
        
        if (bulletPrefab != null && firePoint != null)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            Vector2 shootDirection = (mousePos - firePoint.position).normalized;

            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));
            
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = shootDirection * bulletSpeed; 
        }
    }
}