using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Portal : MonoBehaviour
{
    public string nextSceneName;

    // Tạo một ô nhập tọa độ X Y trên Inspector
    public Vector2 spawnPosition;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Báo GameManager chuyển map và đưa tọa độ cho nó
            GameManager.instance.RequestTransition(nextSceneName, spawnPosition);
        }
    }
}