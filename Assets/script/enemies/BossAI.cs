using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using FirstGearGames.SmoothCameraShaker;

public class BossAI : MonoBehaviour
{
    [Header("QUẢN LÝ MÔI TRƯỜNG")]
    public EnvironmentManager envManager;
    public ShakeData deathShakeData;
    public GameObject portal;
    public enum BossState { Idle, Skill1_Melee, Skill2_Spiral, Skill3_Meteor }

    [Header("TRẠNG THÁI HIỆN TẠI")]
    public BossState currentState;

    [Header("THÔNG SỐ DI CHUYỂN")]
    public float walkSpeed = 2f;
    public float dashSpeed = 10f;
    public float restTime = 2f;

    [Header("MÁU & BỊ THƯƠNG")]
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDead = false;
    public Slider healthSlider; 
    private Vector3 healthBarScale; 

    [Header("CHIÊU 1: CẬN CHIẾN")]
    public float meleeAttackRange = 1.5f;
    public float meleeDamage = 20f;
    public float chaseTimeout = 5f; 

    [Header("CHIÊU 2: BÃO ĐẠN XOẮN ỐC")]
    public Transform mapCenter; 
    public GameObject bulletPrefab; 
    public int waves = 10; 
    public int bulletsPerWave = 8; 
    public float timeBetweenWaves = 0.2f;

    [Header("CHIÊU 3: MƯA THIÊN THẠCH")]
    public GameObject meteorPrefab; 
    public int meteorCount = 8;
    public float timeBetweenMeteors = 0.5f;
    public float dropRadius = 5f; 

    private Transform player;
    private Animator anim;
    private SpriteRenderer sr;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
            healthBarScale = healthSlider.transform.localScale;
        }

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        StartCoroutine(BossLogicLoop());
    }


    IEnumerator BossLogicLoop()
    {
        yield return new WaitForSeconds(1f);

        while (!isDead && player != null)
        {
            currentState = BossState.Idle;
            if (anim != null) anim.SetBool("isRunning", false);
            yield return new WaitForSeconds(restTime);

            int randomSkill = Random.Range(1, 4); 

            if (randomSkill == 1) yield return StartCoroutine(Skill1_MeleeAttack());
            else if (randomSkill == 2) yield return StartCoroutine(Skill2_SpiralBullets());
            else if (randomSkill == 3) yield return StartCoroutine(Skill3_MeteorStrike());
        }
    }

   
    IEnumerator Skill1_MeleeAttack()
    {
        currentState = BossState.Skill1_Melee;
        float chaseTimer = 0f;
        if (anim != null) anim.SetBool("isRunning", true);

        while (Vector2.Distance(transform.position, player.position) > meleeAttackRange && chaseTimer < chaseTimeout)
        {
            chaseTimer += Time.deltaTime;
            MoveTowardsTarget(player.position, walkSpeed);
            yield return null;
        }

        if (anim != null) anim.SetBool("isRunning", false);

        if (Vector2.Distance(transform.position, player.position) <= meleeAttackRange)
        {
            LookAtTarget(player.position);
            if (anim != null) anim.SetTrigger("MeleeAttack");

            if (GlobalAudioManager.Instance != null) GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.bossMelee);

            yield return new WaitForSeconds(0.3f);

            if (Vector2.Distance(transform.position, player.position) <= meleeAttackRange && !isDead)
            {
                player.GetComponent<Health>().TakeDamage(meleeDamage);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Skill2_SpiralBullets()
    {
        currentState = BossState.Skill2_Spiral;
        if (mapCenter == null) yield break;

        float dashTimer = 0f;
        float dashTimeout = 2.5f;

        if (anim != null) anim.SetBool("isFlying", true);

        while (Vector2.Distance(transform.position, mapCenter.position) > 0.1f && dashTimer < dashTimeout)
        {
            dashTimer += Time.deltaTime;
            MoveTowardsTarget(mapCenter.position, dashSpeed); 
            yield return null;
        }

        if (anim != null) anim.SetBool("isFlying", false);

        yield return new WaitForSeconds(0.5f);

        float angleStep = 360f / bulletsPerWave;
        float spiralShift = 10f;

        Vector2 dirToPlayer = player.position - transform.position;
        float baseAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < waves; i++)
        {
            if (anim != null) anim.SetTrigger("CastSpell");

            if (GlobalAudioManager.Instance != null) GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.bossSpiralShoot);

            for (int j = 0; j < bulletsPerWave; j++)
            {
                float currentAngle = baseAngle + (j * angleStep);
                Vector2 bulletDir = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad));

                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null) rb.linearVelocity = bulletDir * 5f;
            }

            baseAngle += spiralShift;
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator Skill3_MeteorStrike()
    {
        currentState = BossState.Skill3_Meteor;
        if (anim != null) anim.SetTrigger("CastSpell");

        for (int i = 0; i < meteorCount; i++)
        {
            if (GlobalAudioManager.Instance != null) GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.bossMeteor);

            Vector2 randomDropPos = (Vector2)player.position + Random.insideUnitCircle * dropRadius;
            Instantiate(meteorPrefab, randomDropPos, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenMeteors);
        }
        yield return new WaitForSeconds(1f);
    } 
    
    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;
        if (healthSlider != null) healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            BossDie();
        }
        else
        {
            if (GlobalAudioManager.Instance != null) GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.bossHurt);

            bool isMoving = false;
            if (anim != null)
            {
                isMoving = anim.GetBool("isRunning") || anim.GetBool("isFlying");
            }

            if (currentState == BossState.Idle || isMoving)
            {
                if (anim != null) anim.SetTrigger("Hurt");
            }
            else
            {
                if (sr != null) StartCoroutine(FlashWhite());
            }
        }
    }
    IEnumerator FlashWhite()
    {
        sr.color = new Color(1f, 1f, 1f, 0.3f); 
        
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white; 
    }

    void BossDie()
    {
        isDead = true;
        StopAllCoroutines();

        if (GlobalAudioManager.Instance != null)
        {
            if (GlobalAudioManager.Instance.bgmSource != null) GlobalAudioManager.Instance.bgmSource.Stop();

            GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.bossDie);

            GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.victory);
        }

        if (anim != null) anim.SetBool("isDead", true);

        Collider2D coll = GetComponent<Collider2D>();
        if (coll != null) coll.enabled = false;
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        if (deathShakeData != null)
        {
            CameraShakerHandler.Shake(deathShakeData);
        }

        if (envManager != null)
        {
            envManager.OnBossDefeated();
        }

        if (portal != null)
        {
            portal.SetActive(true);
        }
        
        Destroy(gameObject, 3f);
    }

    void MoveTowardsTarget(Vector2 target, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        LookAtTarget(target);
    }

    void LookAtTarget(Vector2 target)
    {
        if (target.x > transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (target.x < transform.position.x)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void LateUpdate()
    {
        if (healthSlider != null)
        {
            healthSlider.transform.localScale = new Vector3(
                healthBarScale.x * Mathf.Sign(transform.localScale.x),
                healthBarScale.y,
                healthBarScale.z
            );
        }
    }
}
