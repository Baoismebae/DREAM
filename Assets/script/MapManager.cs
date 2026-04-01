using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.Cinemachine;

public class MapManager : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!string.IsNullOrEmpty(SceneTransition.targetSpawnPoint))
        {
            StartCoroutine(TeleportPlayer());
        }
    }

    IEnumerator TeleportPlayer()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject spawnPoint = GameObject.Find(SceneTransition.targetSpawnPoint);

        if (player != null && spawnPoint != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null) rb.linearVelocity = Vector2.zero;

            // 1. Dịch chuyển Player
            player.transform.position = new Vector3(
                spawnPoint.transform.position.x, 
                spawnPoint.transform.position.y, 
                0f
            );

            // ==========================================
            // 🌟 2. CHỮA BỆNH MẤT TRÍ NHỚ CHO CAMERA
            // ==========================================
            // Tìm cái Camera trong Map 3
            CinemachineVirtualCamera vCam = FindFirstObjectByType<CinemachineVirtualCamera>();
            
            if (vCam != null)
            {
                // Ép nó đi theo con Player vừa mới chuyển sang
                vCam.Follow = player.transform;
                Debug.Log("🎥 Camera đã khóa mục tiêu vào Player mới!");
            }
            // ==========================================

            SceneTransition.targetSpawnPoint = "";
        }
    }
}