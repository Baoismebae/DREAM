using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar;

    [HideInInspector] public bool isInvincible = false; // Trạng thái bất tử khi bật khiên

    private Animator anim;
    private bool isDead = false;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (isDead || isInvincible) return; // Nếu chết hoặc đang bật khiên thì không mất máu

        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0) Die();
        else if (anim != null) anim.SetTrigger("Hurt");
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        if (healthBar != null) healthBar.value = currentHealth / maxHealth;
    }

    void Die() { /* Logic chết của cậu */ }
}