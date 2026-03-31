using UnityEngine;
using UnityEngine.SceneManagement;

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

            // ---> THÊM 1 DÒNG NÀY VÀO ĐÂY: Phát tiếng dịch chuyển <---
            if (GlobalAudioManager.Instance != null)
            {
                GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.teleport);
            }

            SceneManager.LoadScene(sceneName);
        }
    }
}