using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    void Start()
    {
        CheckDoor();
    }

    void CheckDoor()
    {
        if (PlayerPrefs.GetInt("PuzzleSolved", 0) == 1)
        {
            gameObject.SetActive(false); // mở cửa
        }
    }
}