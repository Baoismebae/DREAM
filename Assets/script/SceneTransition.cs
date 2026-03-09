using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [Header("Cài đặt chuyển Map")]
    public string sceneToLoad; // Tên của map tiếp theo (ví dụ: Map2)
    public string spawnPointName; // Tên của điểm xuất hiện ở map mới

    // Biến static để lưu tên điểm cần đến giữa các Scene
    public static string targetSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 1. Lưu lại tên điểm xuất hiện trước khi đổi Map
            targetSpawnPoint = spawnPointName;

            // 2. Load Scene mới
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}