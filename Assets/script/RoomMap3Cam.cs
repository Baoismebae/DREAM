using UnityEngine;
using Unity.Cinemachine;

public class RoomMap3Cam : MonoBehaviour
{
    public GameObject virtualCam; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Mage vừa dẫm vào phòng: " + gameObject.name); 

            var vCam = virtualCam.GetComponent<CinemachineCamera>();
           
            if (vCam != null)
            {
                // collision.transform chính là con Player vừa mới dẫm vào cái vạch
                // Gán nó làm mục tiêu theo dõi luôn!
                vCam.Follow = collision.transform; 
            }


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
