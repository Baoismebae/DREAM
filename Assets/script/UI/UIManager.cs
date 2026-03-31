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

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        // Dọn sạch UI lúc mới vào game
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
        for (int i = 0; i < highlights.Length; i++)
        {
            if (highlights[i] != null) highlights[i].SetActive(i == index);
        }
    }

    // CHỈ CẬP NHẬT ĐÚNG CON SỐ, KHÔNG SPRITE RƯỜM RÀ
    public void UpdateCoinText(int coins)
    {
        if (coinTextDisplay != null)
        {
            coinTextDisplay.text = coins.ToString();
        }
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
            if (quantityTexts[slotIndex] != null)
            {
                quantityTexts[slotIndex].text = quantity.ToString();
            }
        }
    }
}