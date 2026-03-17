using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public GameObject visualDoor; // Kéo Object cái cửa vào đây
    private bool canInteract = false;

    void Start()
    {
        // Nếu đã thắng Sokoban rồi thì xóa cửa ngay khi load Map 3
        if (GameData.isSokobanWon)
        {
            visualDoor.SetActive(false);
        }
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("Scene_Sokoban");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) canInteract = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) canInteract = false;
    }
}