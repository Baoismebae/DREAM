using UnityEngine;
using Unity.Cinemachine;
using FirstGearGames.SmoothCameraShaker;

public class BossSummoner : MonoBehaviour
{
   [Header("ĐỐI TƯỢNG CẦN GỌI")]
    public GameObject bossObject; 
    public GameObject interactUI; 
    
    [Header("CAMERA & HIỆU ỨNG")]
    public CinemachineCamera bossCamera;
    
    // 2. Thêm cái hộp để chứa data rung
    public ShakeData explosionShakeData; 

    private bool isPlayerInRange = false; 
    private bool hasSummoned = false;     

    void Start()
    {
        if (bossObject != null) bossObject.SetActive(false);
        if (interactUI != null) interactUI.SetActive(false);
        
        if (bossCamera != null) bossCamera.Priority = 0; 
    }

    void Update()
    {
        if (isPlayerInRange && !hasSummoned && Input.GetKeyDown(KeyCode.E))
        {
            SummonBoss();
        }
    }

    void SummonBoss()
    {
        hasSummoned = true;

        if (interactUI != null) interactUI.SetActive(false);

        if (bossCamera != null)
        {
            bossCamera.Priority = 20;
        }

        if (bossObject != null)
        {
            bossObject.SetActive(true);
            Debug.Log("Đã triệu hồi Boss thành công!");

            // 3. GỌI RUNG LẮC NGAY LÚC BOSS VỪA XUẤT HIỆN
            if (explosionShakeData != null)
            {
                CameraShakerHandler.Shake(explosionShakeData);
            }
            else
            {
                Debug.LogWarning("Chưa kéo file ShakeData vào Bệ Đá nha Lem Dúa ơi!");
            }

            // ==========================================
            // 🌟 ĐÃ THÊM: ĐỔI NHẠC VÀ PHÁT TIẾNG GẦM
            // ==========================================
            if (GlobalAudioManager.Instance != null)
            {
                // 1. Dập tắt nhạc đồng quê, bật nhạc Boss dồn dập
                GlobalAudioManager.Instance.PlayBGM(GlobalAudioManager.Instance.bossFightBGM);

                // 2. Kèm theo tiếng Boss gầm thét rung trời
                GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.bossRoar);
            }
            // ==========================================
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !hasSummoned)
        {
            isPlayerInRange = true;
            if (interactUI != null) interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (interactUI != null) interactUI.SetActive(false); 
        }
    }
}
