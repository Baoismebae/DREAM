using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleManager : MonoBehaviour
{
    public static PuzzleManager instance;

    public int totalGoals = 3;
    int completedGoals = 0;

    void Awake()
    {
        instance = this;
    }

    public void GoalCompleted()
    {
        completedGoals++;

        if (completedGoals >= totalGoals)
        {
            SceneManager.LoadScene("Map3");
        }
    }

    public void ResetPuzzle()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}