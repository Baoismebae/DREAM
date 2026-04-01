using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "DREAM/Item Data")]
public class ItemData : ScriptableObject
{
    // Thêm 'None' lên đầu tiên
    public enum ItemType { None, Health, Speed, Shield }

    [Header("THÔNG SỐ CƠ BẢN")]
    public string itemName;
    public ItemType type;
    public int cost;
    public Sprite itemIcon;      // Cái này rất quan trọng để hiện lên túi đồ!

    [Header("THÔNG SỐ TÁC DỤNG")]
    public float duration;
    public float potency;

    [Header("ÂM THANH KHI DÙNG")]
    public AudioClip useSound;
}