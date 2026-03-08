using UnityEngine;

public class StaticObjectSort : MonoBehaviour
{
    void Start()
    {
        // Tự động tính Sorting Order một lần duy nhất khi game bắt đầu
        // Chúng ta dùng nhân 100 để tạo ra dải số rộng, tránh các vật quá gần nhau bị trùng số
        GetComponent<SpriteRenderer>().sortingOrder = (int)(-transform.position.y * 100);
        
        // Sau khi tính xong thì xóa script này đi để tối ưu bộ nhớ
        Destroy(this);
    }
}