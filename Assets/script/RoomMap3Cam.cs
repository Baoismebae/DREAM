using UnityEngine;

public class RoomMap3Cam : MonoBehaviour
{
    public GameObject virtualCam; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Báo lên Console để tụi mình biết Mage đã dẫm vào
            Debug.Log("Mage vừa dẫm vào phòng: " + gameObject.name); 
            virtualCam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Mage đã rời khỏi phòng: " + gameObject.name);
            virtualCam.SetActive(false);
        }
    }
}
