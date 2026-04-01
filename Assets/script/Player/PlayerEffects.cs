using System.Collections;
using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    [Header("Particle Systems")]
    public GameObject healParticles;   // Kéo object HealParticles (màu xanh lục) vào đây
    public GameObject speedParticles;  // Kéo object SpeedParticles (màu xanh lam) vào đây
    public GameObject shieldParticles; // Kéo object ShieldParticles (màu trắng) vào đây

    [Header("Effect Settings")]
    public float healEffectDuration = 1.0f; // Thời gian chớp sáng của hạt hồi máu

    // Gọi hàm này khi Frankie nhặt được item Hồi máu
    public void ApplyHealEffect()
    {
        StartCoroutine(PlayParticle(healParticles, healEffectDuration));
        // TODO: Thêm logic cộng máu thực tế vào đây
    }

    // Gọi hàm này khi nhặt item Tốc độ (hữu ích để chạy qua các bẫy giải đố nhanh hơn)
    public void ApplySpeedEffect(float duration)
    {
        StartCoroutine(PlayParticle(speedParticles, duration));
        // TODO: Thêm logic tăng speed thực tế vào đây
    }

    // Gọi hàm này khi nhặt item Khiên
    public void ApplyShieldEffect(float duration)
    {
        StartCoroutine(PlayParticle(shieldParticles, duration));
        // TODO: Thêm logic bật trạng thái vô địch/cộng giáp vào đây
    }

    // Coroutine dùng chung để bật và tự động tắt Particle sau một khoảng thời gian
    private IEnumerator PlayParticle(GameObject particleObject, float duration)
    {
        if (particleObject != null)
        {
            // Bật Object lên (Particle System có Play On Awake sẽ tự động chạy)
            particleObject.SetActive(true);

            // Đợi cho đến khi hết thời gian tác dụng của buff
            yield return new WaitForSeconds(duration);

            // Tắt Object đi
            particleObject.SetActive(false);
        }
    }
}