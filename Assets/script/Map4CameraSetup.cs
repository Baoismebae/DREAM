using UnityEngine;
using System.Collections;
using Unity.Cinemachine; // Thư viện Camera của Unity 6

public class Map4CameraSetup : MonoBehaviour
{
    IEnumerator Start()
    {
        // 🌟 Bí quyết: Chờ 2 khung hình để con Player từ Map 2 kịp bước sang Map 4
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Đi tìm con Player đang sống sờ sờ trong Map
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Lấy luôn cái Virtual Camera đang gắn chung với script này
            var vCam = GetComponent<CinemachineCamera>();

            if (vCam != null)
            {
                // Chỉ đạo Camera bám theo Player
                vCam.Follow = player.transform;
                Debug.Log("✅ [Map 4] Camera đã khóa mục tiêu vào Player!");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ [Map 4] Tìm mỏi mắt không thấy Player đâu!");
        }
    }
}