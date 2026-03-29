using UnityEngine;

public class PersistentAudio : MonoBehaviour
{
    // Biến static này dùng để ghi nhớ xem đã có bản nhạc nào đang phát chưa
    private static PersistentAudio instance;

    void Awake()
    {
        // Nếu đã có một cái máy phát nhạc tồn tại rồi...
        if (instance != null)
        {
            // ...thì tự hủy chính nó (cái mới sinh ra) để tránh 2 bài phát chồng lên nhau
            Destroy(gameObject);
            return;
        }

        // Nếu chưa có, nó sẽ tự nhận nó là bản gốc
        instance = this;

        // Và ra lệnh cho Unity: "Đừng tiêu diệt tôi khi chuyển Scene!"
        DontDestroyOnLoad(gameObject);
    }
}