using UnityEngine;

public class RunEffect : MonoBehaviour
{
    [Header("GẮN HIỆU ỨNG CON")]
    public ParticleSystem runDustParticles;

    [Header("THÔNG SỐ CHỈNH SỬA")]
    public float runSpeedThreshold = 1.0f; // Tui hạ xuống 1 xíu cho dễ ra bụi

    private Rigidbody2D rb;
    private ParticleSystem.EmissionModule emissionModule; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (runDustParticles != null)
        {
            emissionModule = runDustParticles.emission;
            emissionModule.enabled = false; 
        }
    }

    void Update()
    {
        if (rb == null || runDustParticles == null) return;

       
        float currentSpeed = rb.linearVelocity.magnitude; 
        
        
        bool isRunning = currentSpeed > runSpeedThreshold;

        // Bật/Tắt phun bụi
        if (isRunning)
        {
            if (!emissionModule.enabled) emissionModule.enabled = true;
        }
        else
        {
            if (emissionModule.enabled) emissionModule.enabled = false;
        }
    }
}
