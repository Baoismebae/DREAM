using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTransition : MonoBehaviour
{
  [Header("CÀI ĐẶT ĐIỂM ĐẾN")]
    public string sceneToLoad;       // Tên Map muốn qua (VD: "Map3")
    public string spawnPointName;    // Tên cục SpawnPoint bên kia (VD: "SpawnPoint")

    // Biến static để truyền thông tin sang MapManager
    public static string targetSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Gửi tên điểm đến vào "hộp thư" cho MapManager
            targetSpawnPoint = spawnPointName;

            // 2. Phát tiếng dịch chuyển (Lấy từ kịch bản cũ của bạn)
            if (GlobalAudioManager.Instance != null)
            {
                GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.teleport);
            }

            Debug.Log("Đang chuyển sang map: " + sceneToLoad + " | Điểm đến: " + spawnPointName);

            // 3. Thực hiện chuyển Map (Chỉ gọi 1 lần duy nhất ở đây)
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}