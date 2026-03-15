using UnityEngine;
using UnityEngine.SceneManagement; // Để chuyển cảnh và load lại game

public class MenuManager : MonoBehaviour
{
    public GameObject endMenuUI; // Kéo cái Panel EndGameMenu vào đây
    private bool isPaused = false;

    void Update()
    {
        // Kiểm tra nếu nhấn nút ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    // Hàm Tiếp tục (Dùng cho nút Resume và phím ESC)
    public void Resume()
    {
        endMenuUI.SetActive(false);
        Time.timeScale = 1f; // Chạy lại thời gian trong game
        isPaused = false;
    }

    // Hàm Tạm dừng (Dùng cho phím ESC)
    void Pause()
    {
        endMenuUI.SetActive(true);
        Time.timeScale = 0f; // Dừng mọi chuyển động trong game
        isPaused = true;
    }

    // Hàm Chơi lại (Dùng cho nút Restart)
    public void Restart()
    {
        Time.timeScale = 1f; // Phải reset thời gian trước khi load cảnh mới
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Load lại cảnh hiện tại
    }

    // Hàm về Menu (Dùng cho nút Main Menu)
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene"); // Thay tên cảnh Menu của cậu vào đây
    }

    // Hàm khi Player chết (Để script PlayerHealth gọi tới)
    public void OnPlayerDie()
    {
        endMenuUI.SetActive(true);
        Time.timeScale = 0f;
        // Có thể ẩn nút Resume đi vì chết rồi thì không tiếp tục được
    }
}