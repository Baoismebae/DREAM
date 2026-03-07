using UnityEngine;

public class PersistentPlayer : MonoBehaviour
{
    private static PersistentPlayer instance;

    private void Awake()
    {
        // Kiểm tra xem đã có con Player nào tồn tại chưa (Singleton Pattern)
        if (instance == null)
        {
            instance = this;
            // Lệnh quan trọng nhất: Giữ vật thể này không bị xóa khi chuyển Scene
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            // Nếu đã có một con từ Map trước mang sang rồi, thì xóa con mới ở Map này đi
            Destroy(gameObject);
        }
    }
}