using UnityEngine;

public class StaticObjectSort : MonoBehaviour
{
    void Start() {
    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    if(sr != null) {
        // Ép nó về cùng Layer với Player
        sr.sortingLayerName = "Default"; 
        // Lấy đúng vị trí chân (Pivot đã chỉnh ở Bottom)
        sr.sortingOrder = (int)(-transform.position.y * 1000);
    }
    Destroy(this);
}
}