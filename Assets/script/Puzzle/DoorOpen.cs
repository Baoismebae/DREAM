using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleOpen : MonoBehaviour
{
    bool playerNear = false;

    void Update()
    {
        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("PuzzleScene");
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerNear = true;
            Debug.Log("Press E to enter puzzle");
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}