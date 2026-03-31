using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Đặt tên unique cho mỗi điểm, ví dụ "Map3_A", "Map4_Start", "Map4_DoorToMap3"
    // ID này phải trùng với ID mà Portal chỉ định.
    public string spawnPointId;

    // Hiển thị một icon nhỏ trong Scene View để dễ nhìn vị trí đặt.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}