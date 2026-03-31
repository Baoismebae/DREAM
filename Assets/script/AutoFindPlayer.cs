using UnityEngine;
using Unity.Cinemachine; // Bắt buộc phải có dòng này cho Unity 6

public class AutoFindPlayer : MonoBehaviour
{
    void Start()
    {
        // Tự động tìm object có Tag là "Player" mỗi khi Map được load
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Tìm component Cinemachine Camera và gán Player vào ô Follow
            CinemachineCamera cam = GetComponent<CinemachineCamera>();
            if (cam != null)
            {
                cam.Follow = player.transform;
            }
        }
    }
}