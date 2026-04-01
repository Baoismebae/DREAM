using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // BẮT BUỘC: Thêm thư viện quản lý Scene

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Bật/Tắt UI theo Scene")]
    // Kéo cái object chứa toàn bộ UI (hoặc kéo thẳng cái Canvas) vào đây
    public GameObject mainUIContainer;

    [Header("UI Tiền")]
    public TextMeshProUGUI coinTextDisplay;

    [Header("Kho đồ (Inventory 4 ô)")]
    public Image[] itemIcons;
    public GameObject[] highlights;
    public TextMeshProUGUI[] quantityTexts;

    private int selectedSlot = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ UI không bị mất khi qua map

            // Đăng ký sự kiện: Lắng nghe mỗi khi load xong 1 Scene mới
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Gỡ sự kiện khi object bị hủy để tránh lỗi
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // HÀM MỚI: Tự động chạy mỗi khi chuyển Map
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Nhớ đổi chữ "SokobanScene" thành tên chính xác của Scene Sokoban của Aly nhé!
        if (scene.name == "SokobanScene")
        {
            if (mainUIContainer != null) mainUIContainer.SetActive(false); // Giấu UI đi
        }
        else
        {
            if (mainUIContainer != null) mainUIContainer.SetActive(true); // Hiện UI lại ở Map khác
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
        selectedSlot = index;
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

    public int GetSelectedSlot()
    {
        return selectedSlot;
    }

    public void ConsumeItemUI(int slotIndex, int newQuantity)
    {
        if (slotIndex >= 0 && slotIndex < itemIcons.Length)
        {
            if (newQuantity <= 0)
            {
                if (itemIcons[slotIndex] != null) itemIcons[slotIndex].enabled = false;
                if (quantityTexts[slotIndex] != null) quantityTexts[slotIndex].text = "";
            }
            else
            {
                if (quantityTexts[slotIndex] != null) quantityTexts[slotIndex].text = newQuantity.ToString();
            }
        }
    }
}