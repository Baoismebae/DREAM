using UnityEngine;

public class UIPulsing : MonoBehaviour
{
    // Cần phải có chữ 'public' ở phía trước thì ô này mới hiện ra ở Unity
    public float speed = 5f; 
    
    // Ô này để cậu kéo cái Canvas Group vào
    public CanvasGroup cg;

    void Update()
    {
        if (cg != null)
        {
            // Công thức Sin giúp tạo nhịp nhấp nháy
            cg.alpha = (Mathf.Sin(Time.time * speed) + 1f) / 2f;
        }
    }
}