using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    public enum BossState { Idle, Skill1_Melee, Skill2_Spiral, Skill3_Meteor }
    [Header("TRẠNG THÁI HIỆN TẠI")]
    public BossState currentState;

    [Header("THÔNG SỐ DI CHUYỂN")]
    public float walkSpeed = 2f;
    public float dashSpeed = 10f;
    public float restTime = 2f; // Thời gian nghỉ giữa các chiêu

    [Header("MÁU & BỊ THƯƠNG")]
    public float maxHealth = 100f; // Boss thì máu phải trâu!
    private float currentHealth;
    private bool isDead = false;
    public Slider healthSlider; // Kéo Slider thanh máu vào đây
    private Vector3 healthBarScale; // Lưu kích thước để chống lật thanh máu

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
        
        // --- SETUP MÁU & THANH MÁU ---
        currentHealth = maxHealth;
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
            healthBarScale = healthSlider.transform.localScale; // Lưu lại scale gốc
        }

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        // Bắt đầu vòng lặp chiến đấu của Boss
        StartCoroutine(BossLogicLoop());
    }

    // =========================================================
    // VÒNG LẶP CHIẾN ĐẤU CHÍNH (TRÍ NÃO CỦA BOSS)
    // =========================================================
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

    // =========================================================
    // CÁC CHIÊU THỨC CỦA BOSS
    // =========================================================
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

        // ==========================================
        // 1. LƯỚT RA GIỮA MAP (CÓ CẦU DAO TỰ NGẮT)
        // ==========================================
        float dashTimer = 0f;
        float dashTimeout = 2.5f; // Tối đa 2.5 giây để lướt. Chạy không kịp cũng phải đứng lại bắn!

        if (anim != null) anim.SetBool("isFlying", true);

        // Thêm điều kiện dashTimer < dashTimeout vào vòng lặp
        while (Vector2.Distance(transform.position, mapCenter.position) > 0.1f && dashTimer < dashTimeout)
        {
            dashTimer += Time.deltaTime; // Đồng hồ đếm giờ chạy
            MoveTowardsTarget(mapCenter.position, dashSpeed); 
            yield return null;
        }

        if (anim != null) anim.SetBool("isFlying", false);

        yield return new WaitForSeconds(0.5f);

        // ==========================================
        // 2. TÍNH TOÁN GÓC BẮN 360 ĐỘ (Giữ nguyên của bạn)
        // ==========================================
        float angleStep = 360f / bulletsPerWave; 
        float spiralShift = 10f; 

        Vector2 dirToPlayer = player.position - transform.position;
        float baseAngle = Mathf.Atan2(dirToPlayer.y, dirToPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < waves; i++)
        {
            if (anim != null) anim.SetTrigger("CastSpell"); 

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
            Vector2 randomDropPos = (Vector2)player.position + Random.insideUnitCircle * dropRadius;
            Instantiate(meteorPrefab, randomDropPos, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenMeteors);
        }
        yield return new WaitForSeconds(1f); 
    }

    // =========================================================
    // HỆ THỐNG MÁU & NHẬN SÁT THƯƠNG
    // =========================================================
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
                // Nếu đang vung kiếm hoặc niệm phép -> Bật Giáp Bá Thể (Chỉ chớp nháy, không giật mình)
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
        
        // Dừng tất cả các chiêu thức đang xài dang dở
        StopAllCoroutines(); 

        if (anim != null) anim.SetBool("isDead", true);
        
        // Tắt va chạm để không làm vướng đường Player
        Collider2D coll = GetComponent<Collider2D>();
        if (coll != null) coll.enabled = false;

        // Tắt thanh máu
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        // Chờ diễn anim xong rồi mới xóa Boss (Có thể chỉnh lại thời gian này)
        Destroy(gameObject, 3f); 
    }

    // =========================================================
    // HÀM HỖ TRỢ DI CHUYỂN, LẬT MẶT & CHỐNG LẬT THANH MÁU
    // =========================================================
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
        // Ép thanh máu luôn hiển thị thẳng, bất kể Boss quay đầu đi đâu
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
