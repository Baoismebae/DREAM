using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool isSokobanSolved = false;

    // Chỉ cần lưu tọa độ X, Y
    private Vector2 targetPosition;
    private bool shouldMovePlayer = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Nếu đã có 1 cái từ map trước truyền sang, cái mới sẽ tự sát
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (instance == null) {
    instance = this;
    DontDestroyOnLoad(gameObject);
} else {
    Destroy(gameObject); // Nếu đã có 1 cái từ map trước truyền sang, cái mới sẽ tự sát
}
    }

    // Cửa sẽ truyền tên Map mới và tọa độ muốn nhảy tới vào đây
    public void RequestTransition(string nextSceneName, Vector2 newPosition)
    {
        targetPosition = newPosition;
        shouldMovePlayer = true;
        SceneManager.LoadScene(nextSceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (shouldMovePlayer)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // Bê Player đặt đúng vào tọa độ đã nhập, ép Z = 0
                player.transform.position = new Vector3(targetPosition.x, targetPosition.y, 0f);
                shouldMovePlayer = false; // Xóa trạng thái để không bị lặp
            }
        }
    }
}