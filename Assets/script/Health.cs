using UnityEngine;
using UnityEngine.UI; // Để dùng thanh máu Slider

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public Slider healthBar; // Kéo Slider máu vào đây

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
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
        // Nếu là Player thì có thể load lại map, nếu là quái thì biến mất
        if (gameObject.CompareTag("Player"))
        {
            Debug.Log("Mage đã hy sinh!");
            // Thêm logic hồi sinh ở đây
        }
        else
        {
            Destroy(gameObject); // Quái biến mất
        }
    }
}