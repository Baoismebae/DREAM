using UnityEngine;
using UnityEngine.SceneManagement;

public class Map3Door : MonoBehaviour
{
    public GameObject closedDoorVisual; // Hình ảnh cửa đóng
    public GameObject openedDoorVisual; // Hình ảnh cửa mở (hoặc đường đi)

    void Start()
    {
        // Khi load lại Map 3, kiểm tra xem đã giải xong Sokoban chưa
        if (GameProgress.isMap3DoorOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu cửa chưa mở và player chạm vào trigger -> Load minigame
        if (other.CompareTag("Player") && !GameProgress.isMap3DoorOpen)
        {
            // Nhớ thêm "SokobanScene" vào File -> Build Settings
            SceneManager.LoadScene("SokobanScene");
        }
    }

    void OpenDoor()
    {
        closedDoorVisual.SetActive(false);
        openedDoorVisual.SetActive(true);
        // Tắt collider chặn đường để player đi qua
        GetComponent<Collider2D>().enabled = false;
    }

    void CloseDoor()
    {
        closedDoorVisual.SetActive(true);
        openedDoorVisual.SetActive(false);
    }
}