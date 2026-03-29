using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
        GameManager.isSokobanSolved = true;

        // Phát nhạc thắng
        if (SokobanAudioManager.Instance != null) SokobanAudioManager.Instance.PlaySFX(SokobanAudioManager.Instance.winSound);

        // Trì hoãn 1 giây rồi mới về map 3 để nghe nhạc
        StartCoroutine(DelayLoadScene(map3SceneName, 1f));
    }

    public void ResetGame()
    {
        // Phát tiếng reset
        if (SokobanAudioManager.Instance != null) SokobanAudioManager.Instance.PlaySFX(SokobanAudioManager.Instance.resetSound);

        // Trì hoãn 0.3 giây rồi mới nạp lại scene
        StartCoroutine(DelayLoadScene(SceneManager.GetActiveScene().name, 0.3f));
    }

    IEnumerator DelayLoadScene(string sceneName, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(sceneName);
    }
}