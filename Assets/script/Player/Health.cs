using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
  public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar; 

    private Animator anim;
    private bool isDead = false;

    public MonoBehaviour playerMovementScript; 
    public MonoBehaviour playerAttackScript;

    void Start()
    {
        anim = GetComponentInChildren<Animator>(); 
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (anim != null) anim.SetTrigger("Hurt");

            // ---> THÊM 1 DÒNG NÀY VÀO ĐÂY: Phát tiếng rên/đau <---
            if (GlobalAudioManager.Instance != null) GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.playerHurt);
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    void Die()
    {
        isDead = true;

        // ---> THÊM 1 DÒNG NÀY VÀO ĐÂY: Phát tiếng gục ngã/chết <---
        if (GlobalAudioManager.Instance != null) GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.playerDie);

        // 1. Kích hoạt anim ngã xuống
        if (anim != null) anim.SetTrigger("Dead");

        // 2. KHÓA ĐIỀU KHIỂN: Tắt các script di chuyển và đánh
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (playerAttackScript != null) playerAttackScript.enabled = false;

        // 3. Tắt va chạm (để quái không cắn xác Player nữa)
        GetComponent<Collider2D>().enabled = false;

        // 4. Xử lý Game Over (Ví dụ: Chờ 2 giây rồi tải lại màn chơi)
        Debug.Log("Mage đã hy sinh!");
        Invoke("ReloadGame", 3f); // Gọi hàm ReloadGame sau 3 giây
    }

    void ReloadGame()
    {
       SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}