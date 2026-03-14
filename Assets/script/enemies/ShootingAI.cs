using UnityEngine;
using UnityEngine.UI;

public class ShootingAI : MonoBehaviour
{
   [Header("CẤU HÌNH TUẦN TRA")]
    public Vector2[] patrolPoints = new Vector2[] { new Vector2(-2, -2), new Vector2(2, -2) };
    public float moveSpeed = 3f;
    public float stopDistance = 0.8f;

    [Header("CHIẾN ĐẤU")]
    public float shootingRange = 10f; 
    public float fireRate = 1.5f; // Thời gian đứng im (idle) chờ ở mỗi chu kỳ bắn
    public float minWaitTime = 1f;
    public float maxWaitTime = 3f;

    [Header("CHIẾN ĐẤU TẦM XA")]
    public float detectionRange = 15f;
    public GameObject bullet;
    public GameObject bulletParent; 
    public Transform player;

    [Header("MÁU & BỊ THƯƠNG")]
    public UnityEngine.UI.Slider healthSlider;
    private Vector3 healthBarScale;
    public float maxHealth = 30f;
    private float currentHealth;
    private bool isDead = false;

    [Header("ANIMATION BẮN")]
    public float chargeTime = 1f; 
    private bool isShootingAction = false; 


    private Vector2 startPosition; 
    private Vector2 nextPoint;
    private float waitCounter;
    private bool isWaiting = false;
    private bool isChasing = false;
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth; 
            healthSlider.value = currentHealth; 
            healthBarScale = healthSlider.transform.localScale; 
        }

        if (player == null)
        {
            GameObject targetObj = GameObject.FindGameObjectWithTag("Player");
            if (targetObj != null) player = targetObj.transform;
            
        }
        SetNewPatrolPoint();
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= shootingRange)
        {
            isChasing = false;
            isWaiting = true; 
            ShootLogic(); 
        }
        else if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            isWaiting = false;
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            if (isChasing) 
            { 
                isChasing = false; 
                isWaiting = false; 
                SetNewPatrolPoint(); 
            }
            Patrol();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;
        
        currentHealth -= damageAmount;
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (anim != null) anim.SetTrigger("Hurt");
        }
    }

    void Die()
    {
        isDead = true;
        if (anim != null) anim.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false; // Tắt va chạm

        // Ẩn thanh máu đi cho đỡ vướng mắt khi xác quái còn nằm đó
        if (healthSlider != null) healthSlider.gameObject.SetActive(false);

        Destroy(gameObject, 1.375f); //
    }

    void ShootLogic() 
    {
        float scaleX = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(player.position.x > transform.position.x ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);

        if (!isShootingAction) 
        {
            StartCoroutine(ShootSequence()); 
        }
    }

    System.Collections.IEnumerator ShootSequence()
    {
        isShootingAction = true;

        // 1. Chuyển từ Idle sang Charge
        if (anim != null) anim.SetBool("isCharging", true);

        // 2. Chờ thời gian gồng (chargeTime)
        yield return new WaitForSeconds(chargeTime); 

        // 3. Tạo đạn bay ra
        if (bullet != null && !isDead) 
        {
            Vector2 spawnPos = (bulletParent != null) ? (Vector2)bulletParent.transform.position : (Vector2)transform.position;
            Instantiate(bullet, spawnPos, Quaternion.identity);
        }

        // 4. Bắn xong thì tắt Charge -> Animator sẽ tự động quay về Idle
        if (anim != null) anim.SetBool("isCharging", false);

        // 5. Đứng nghỉ một lúc (Idle) theo biến fireRate rồi mới bắn tiếp
        yield return new WaitForSeconds(fireRate);

        isShootingAction = false;
    }

    void Patrol() 
    {
        if (isWaiting) 
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0) { isWaiting = false; SetNewPatrolPoint(); }
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, nextPoint, moveSpeed * Time.deltaTime);

        if (nextPoint.x > transform.position.x + 0.01f)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (nextPoint.x < transform.position.x - 0.01f)
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        if (Vector2.Distance(transform.position, nextPoint) < 0.2f) 
        {
            isWaiting = true;
            waitCounter = Random.Range(minWaitTime, maxWaitTime);
        }
    }

    void ChasePlayer(float d) 
    { 
        if (d > stopDistance) 
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime); 
            float scaleX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector3(player.position.x > transform.position.x ? scaleX : -scaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    void SetNewPatrolPoint() 
    {
        if (patrolPoints.Length < 1) return;
        nextPoint = startPosition + patrolPoints[Random.Range(0, patrolPoints.Length)];
    }

    void LateUpdate()
    {
        if (healthSlider != null)
        {
            // Ép thanh máu luôn hiển thị đúng chiều dương, bất kể quái quay đi đâu
            healthSlider.transform.localScale = new Vector3(
                healthBarScale.x * Mathf.Sign(transform.localScale.x),
                healthBarScale.y,
                healthBarScale.z
            );
        }
    }
}
