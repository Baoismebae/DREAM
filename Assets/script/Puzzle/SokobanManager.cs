using UnityEngine;
using UnityEngine.SceneManagement;

public class SokobanManager : MonoBehaviour
{
    public static SokobanManager Instance;

    private int totalGoals;
    private int goalsFilled = 0;
    public string map3SceneName = "Map3";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Đếm xem trên map có bao nhiêu Goal
        totalGoals = GameObject.FindGameObjectsWithTag("Goal").Length;
    }

    void Update()
    {
        // Phím tắt R để Reset Scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
    }

    // Crate sẽ gọi hàm này khi nó vào đúng ô Goal
    public void AddFilledGoal()
    {
        goalsFilled++;
        if (goalsFilled >= totalGoals)
        {
            WinMinigame();
        }
    }

    void WinMinigame()
    {
        Debug.Log("Thắng game!");
        GameManager.isSokobanSolved = true; // Lưu trạng thái
        SceneManager.LoadScene(map3SceneName); // Trả về Map 3
    }

    public void ResetGame()
    {
        // Load lại chính Scene hiện tại
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}