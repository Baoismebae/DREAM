using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.GetInt("PuzzleSolved") == 1)
        {
            gameObject.SetActive(false);
        }
    }
}