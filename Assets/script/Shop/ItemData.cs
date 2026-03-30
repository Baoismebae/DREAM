using UnityEngine;

// Lệnh này giúp bạn chuột phải trong cửa sổ Project tạo file data mới
[CreateAssetMenu(fileName = "NewItemData", menuName = "DREAM/Item Data")]
public class ItemData : ScriptableObject
{
    // Phân loại vật phẩm
    public enum ItemType { Health, Speed, Shield }

    [Header("THÔNG SỐ CƠ BẢN")]
    public string itemName;      // Tên vật phẩm (ví dụ: Lọ Hồi Máu)
    public ItemType type;        // Loại vật phẩm
    public int cost;            // Giá tiền
    public Sprite itemIcon;     // Hình ảnh hiển thị trên UI HUD

    [Header("THÔNG SỐ TÁC DỤNG")]
    public float duration;       // Thời gian tác dụng (dùng cho Tốc độ/Khiên)
    public float potency;        // Độ mạnh (ví dụ: 0.25 cho hồi 25% máu, 1.5 cho tăng 1.5 lần tốc độ)

    [Header("ÂM THANH KHI DÙNG")]
    public AudioClip useSound;   // Tiếng uống thuốc, bật khiên...
}