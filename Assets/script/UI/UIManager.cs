using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Tiền")]
    public TextMeshProUGUI coinTextDisplay;

    [Header("Kho đồ (Inventory 4 ô)")]
    public Image[] itemIcons;
    public GameObject[] highlights;
    public TextMeshProUGUI[] quantityTexts;

    private int selectedSlot = 0; // Biến nhớ xem đang chọn ô nào

    void Awake()
    {
        // Kiểm tra xem đã có UIManager nào tồn tại chưa
        if (Instance == null)
        {
            Instance = this;

            // THÊM DÒNG NÀY: Giữ cho HUDCanvas không bị hủy khi qua Map mới
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Nếu lỡ quay lại Map 1 và sinh ra thêm 1 cái HUDCanvas nữa thì hủy cái mới đi
            // Đảm bảo trong game luôn chỉ có duy nhất 1 giao diện
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < quantityTexts.Length; i++)
        {
            if (quantityTexts[i] != null) quantityTexts[i].text = "";
            if (itemIcons[i] != null) itemIcons[i].enabled = false;
        }
        SelectSlot(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
    }

    void SelectSlot(int index)
    {
        selectedSlot = index; // Lưu lại ô đang chọn
        for (int i = 0; i < highlights.Length; i++)
        {
            if (highlights[i] != null) highlights[i].SetActive(i == index);
        }
    }

    public void UpdateCoinText(int coins)
    {
        if (coinTextDisplay != null) coinTextDisplay.text = coins.ToString();
    }

    public void UpdateInventorySlot(int slotIndex, Sprite itemSprite, int quantity)
    {
        if (slotIndex >= 0 && slotIndex < itemIcons.Length)
        {
            if (itemIcons[slotIndex] != null)
            {
                itemIcons[slotIndex].sprite = itemSprite;
                itemIcons[slotIndex].color = Color.white;
                itemIcons[slotIndex].enabled = true;
            }
            if (quantityTexts[slotIndex] != null) quantityTexts[slotIndex].text = quantity.ToString();
        }
    }

    // --- 2 HÀM MỚI ĐỂ DÙNG ĐỒ ---
    public int GetSelectedSlot()
    {
        return selectedSlot; // Trả về số thứ tự ô đang sáng
    }

    public void ConsumeItemUI(int slotIndex, int newQuantity)
    {
        if (slotIndex >= 0 && slotIndex < itemIcons.Length)
        {
            if (newQuantity <= 0) // Nếu dùng hết sạch đồ
            {
                if (itemIcons[slotIndex] != null) itemIcons[slotIndex].enabled = false; // Ẩn hình ảnh
                if (quantityTexts[slotIndex] != null) quantityTexts[slotIndex].text = ""; // Xóa chữ số
            }
            else // Nếu vẫn còn dư
            {
                if (quantityTexts[slotIndex] != null) quantityTexts[slotIndex].text = newQuantity.ToString();
            }
        }
    }
}