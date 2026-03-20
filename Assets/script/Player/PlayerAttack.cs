using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
  [Header("THÔNG SỐ TẤN CÔNG")]
    public float damage = 20f;        
    public float attackRate = 2f;     
    private float nextAttackTime = 0f; 

    [Header("KHÓA DI CHUYỂN")]
    public float attackDuration = 0.4f; 
    private Playermovement movementScript; 

    [Header("CẤU HÌNH VA CHẠM CAPSULE")]
    public Collider2D swordCollider; 
    public LayerMask enemyLayers; 

    [Header("HIỆU ỨNG KHI TRÚNG ĐÒN")]
    public GameObject hitParticlePrefab; 

    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
        movementScript = GetComponent<Playermovement>(); 
    }

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.J) || Input.GetMouseButtonDown(0))
            {
                Attack(); 
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

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
        
        if (swordCollider != null)
        {
            Physics2D.OverlapCollider(swordCollider, filter, hitEnemies);
        }

        foreach (Collider2D enemy in hitEnemies)
        {
            if (hitParticlePrefab != null)
            {
                Destroy(Instantiate(hitParticlePrefab, enemy.transform.position, Quaternion.identity), 0.2f);
            }
        
            ShootingAI rangedEnemy = enemy.GetComponentInParent<ShootingAI>();
            AI meleeEnemy = enemy.GetComponentInParent<AI>();
            BossAI boss = enemy.GetComponentInParent<BossAI>();
            
            // 🌟 1. Gọi tên con Springtrap ra đây
            SpringTrapAI springTrapEnemy = enemy.GetComponentInParent<SpringTrapAI>();

            if (boss != null) boss.TakeDamage(damage); 
            else if (rangedEnemy != null) rangedEnemy.TakeDamage(damage); 
            else if (meleeEnemy != null) meleeEnemy.TakeDamage(damage); 
            
            // 🌟 2. Nếu chém trúng con Springtrap thì gọi hàm trừ máu của nó
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
}