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

    // BIẾN NÀY ĐỂ BẬT KHIÊN
    [HideInInspector] public bool isInvincible = false;

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
        // Nếu đã chết HOẶC đang bật khiên bất tử thì không bị trừ máu
        if (isDead || isInvincible) return;

        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (anim != null) anim.SetTrigger("Hurt");
            if (GlobalAudioManager.Instance != null) GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.playerHurt);
        }
    }

    // HÀM NÀY ĐỂ UỐNG THUỐC HỒI MÁU
    public void Heal(float amount)
    {
        if (isDead) return;

        currentHealth += amount;
        // Không cho hồi vượt quá cục máu tối đa
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        UpdateHealthBar();
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

        if (GlobalAudioManager.Instance != null) GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.playerDie);

        if (anim != null) anim.SetTrigger("Dead");

        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (playerAttackScript != null) playerAttackScript.enabled = false;

        GetComponent<Collider2D>().enabled = false;

        Debug.Log("Mage đã hy sinh!");
        Invoke("ReloadGame", 3f);
    }

    void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}