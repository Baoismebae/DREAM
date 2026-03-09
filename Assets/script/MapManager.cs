using UnityEngine;

public class MapManager : MonoBehaviour
{
    void Start()
    {
        // 1. Tìm con Mage (đối tượng đã dùng DontDestroyOnLoad từ Map 1)
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // 2. Kiểm tra xem trước đó có yêu cầu "điểm đến" nào không
        if (player != null && !string.IsNullOrEmpty(SceneTransition.targetSpawnPoint))
        {
            // 3. Tìm cái Object trống có tên chính xác như yêu cầu (ví dụ: "Diem_Vao_Cua")
            GameObject spawnPoint = GameObject.Find(SceneTransition.targetSpawnPoint);

            if (spawnPoint != null)
            {
                // 4. Đưa Mage tới đúng tọa độ của điểm đó
                player.transform.position = spawnPoint.transform.position;
            }
        }
    }
}