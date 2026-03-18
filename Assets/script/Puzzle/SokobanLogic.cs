using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SokobanLogic : MonoBehaviour
{
    [Header("Cấu hình")]
    public List<SpriteRenderer> goals;
    public Color winColor = Color.green;
    public string winScene = "map3";

    private int completedGoals = 0;

    // Hàm này được gọi mỗi khi 1 Goal xác nhận hoàn thành
    public void GoalOccupied()
    {
        completedGoals++;
        Debug.Log("Tiến độ: " + completedGoals + "/" + goals.Count);

        if (completedGoals >= goals.Count)
        {
            Debug.Log("THẮNG! Đang chuyển cảnh...");
            Invoke("LoadNextScene", 1.2f); // Chờ 1.2 giây cho đẹp
        }
    }

    void LoadNextScene() => SceneManager.LoadScene(winScene);
}