using UnityEngine;
using System.Collections.Generic;

public class SokobanLogic : MonoBehaviour
{
    [Header("Cấu hình")]
    public List<SpriteRenderer> goals; // Kéo 4 Goal vào đây
    public List<GameObject> boxes;    // Kéo 4 Box vào đây
    public Color winColor = Color.green; // Màu khi Box đè lên Goal

    private bool[] isGoalOccupied;

    void Start()
    {
        isGoalOccupied = new bool[goals.Count];
    }

    void Update()
    {
        for (int i = 0; i < goals.Count; i++)
        {
            // Nếu Goal này chưa có Box đè lên
            if (!isGoalOccupied[i])
            {
                foreach (GameObject box in boxes)
                {
                    // Kiểm tra Box còn sống và đè lên Goal không
                    if (box.activeSelf && Vector2.Distance(box.transform.position, goals[i].transform.position) < 0.1f)
                    {
                        box.SetActive(false); // Box biến mất
                        goals[i].color = winColor; // Goal đổi màu
                        isGoalOccupied[i] = true; // Đánh dấu Goal xong

                        CheckWinCondition(); // Kiểm tra xem thắng cả màn chưa
                        break;
                    }
                }
            }
        }
    }

    void CheckWinCondition()
    {
        int count = 0;
        foreach (bool g in isGoalOccupied) if (g) count++;

        if (count >= goals.Count)
        {
            Debug.Log("THẮNG CẢ MÀN!");
            // Gọi lệnh chuyển cảnh quay về Map 3 ở đây
            UnityEngine.SceneManagement.SceneManager.LoadScene("Map3");
        }
    }
}