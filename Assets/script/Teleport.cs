using UnityEngine;
using UnityEngine.SceneManagement; // Thư viện bắt buộc để chuyển cảnh

public class Teleport : MonoBehaviour
{
    [Header("Tên Map muốn chuyển tới")]
    public string sceneName; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem có đúng là Player bước vào không
        if (other.CompareTag("Player"))
        {
            Debug.Log("Đang chuyển sang map: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
    }
}