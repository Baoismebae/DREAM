using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Data")]
    // Mảng chứa 4 ô đồ của người chơi
    public ItemType[] inventorySlots = new ItemType[4] { ItemType.None, ItemType.None, ItemType.None, ItemType.None };

    // Lưu vị trí ô đang được chọn (0 tương đương phím 1, 3 tương đương phím 4)
    public int selectedSlotIndex = 0;

    private PlayerEffects effects;

    void Start()
    {
        // Lấy script tạo Particle đã viết trước đó
        effects = GetComponent<PlayerEffects>();
    }

    void Update()
    {
        // 1. Xử lý đổi ô bằng phím số 1, 2, 3, 4
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);

        // 2. Xử lý dùng item bằng phím Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseCurrentItem();
        }
    }

    // Hàm cập nhật ô đang chọn
    private void SelectSlot(int index)
    {
        selectedSlotIndex = index;
        Debug.Log("Đang chọn ô số: " + (index + 1));

        // TODO: Gọi script quản lý UI ở đây để di chuyển khung viền màu vàng (như trong hình) tới đúng ô tương ứng
    }

    // Hàm sử dụng item
    private void UseCurrentItem()
    {
        ItemType itemToUse = inventorySlots[selectedSlotIndex];

        if (itemToUse != ItemType.None && effects != null)
        {
            // Kích hoạt hiệu ứng tương ứng
            switch (itemToUse)
            {
                case ItemType.Heal:
                    effects.ApplyHealEffect();
                    break;
                case ItemType.Speed:
                    effects.ApplySpeedEffect(5f);
                    break;
                case ItemType.Shield:
                    effects.ApplyShieldEffect(10f);
                    break;
            }

            // Xóa item khỏi túi đồ sau khi dùng
            inventorySlots[selectedSlotIndex] = ItemType.None;
            Debug.Log("Đã dùng item! Ô số " + (selectedSlotIndex + 1) + " giờ đã trống.");

            // TODO: Gọi script UI để xóa hình ảnh (icon) của item khỏi ô ở dưới màn hình
        }
        else
        {
            Debug.Log("Ô này đang trống, không có gì để dùng!");
        }
    }

    // Script ShopItem trên quầy sẽ gọi hàm này khi người chơi MUA HÀNG THÀNH CÔNG
    public bool AddItem(ItemType newItem)
    {
        // Quét xem trong 4 ô có ô nào trống (None) không
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i] == ItemType.None)
            {
                inventorySlots[i] = newItem; // Cho item vào ô trống đầu tiên tìm thấy
                Debug.Log("Đã nhặt item vào ô số: " + (i + 1));

                // TODO: Gọi script UI cập nhật icon item vào đúng khung hiển thị

                return true; // Báo về là nhặt thành công (để ShopItem biết mà xóa hàng trên quầy)
            }
        }

        Debug.Log("Túi đồ đã đầy 4 ô!");
        return false; // Báo về nhặt thất bại vì đầy túi
    }
}