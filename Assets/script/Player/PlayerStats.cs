using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [Header("Cài đặt tiền tệ")]
    public int currentCoins = 0;

    [Header("TÚI ĐỒ (INVENTORY)")]
    public Dictionary<ItemData.ItemType, int> inventory = new Dictionary<ItemData.ItemType, int>();

    void Awake()
    {
        instance = this;
        inventory[ItemData.ItemType.Health] = 0;
        inventory[ItemData.ItemType.Speed] = 0;
        inventory[ItemData.ItemType.Shield] = 0;
    }

    void Start()
    {
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
    }

    public bool SpendCoins(int amount)
    {
        if (currentCoins >= amount)
        {
            currentCoins -= amount;
            UpdateCoinUI();
            return true;
        }
        return false;
    }

    void UpdateCoinUI()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateCoinText(currentCoins);
        }
    }

    public bool TryBuyItem(ItemData itemData)
    {
        if (SpendCoins(itemData.cost))
        {
            inventory[itemData.type]++;
            int slotIndex = GetSlotIndex(itemData.type);

            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateInventorySlot(slotIndex, itemData.itemIcon, inventory[itemData.type]);
            }

            if (GlobalAudioManager.Instance != null)
                GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.buySuccessSound);

            return true;
        }

        if (GlobalAudioManager.Instance != null)
            GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.buyFailSound);

        return false;
    }

    private int GetSlotIndex(ItemData.ItemType type)
    {
        switch (type)
        {
            case ItemData.ItemType.Health: return 0; // Ô số 1 (Slot 1)
            case ItemData.ItemType.Speed: return 1;  // Ô số 2 (Slot 2)
            case ItemData.ItemType.Shield: return 2; // Ô số 3 (Slot 3)
            default: return 3;                       // Ô số 4 (Slot 4)
        }
    }
}