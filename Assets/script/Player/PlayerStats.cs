using System.Collections;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseSelectedItem();
        }
    }

    private void UseSelectedItem()
    {
        if (UIManager.Instance == null) return;

        int currentSlot = UIManager.Instance.GetSelectedSlot();
        ItemData.ItemType typeToUse = GetItemTypeBySlot(currentSlot);

        if (inventory.ContainsKey(typeToUse) && inventory[typeToUse] > 0)
        {
            inventory[typeToUse]--;
            UIManager.Instance.ConsumeItemUI(currentSlot, inventory[typeToUse]);
            ApplyItemEffect(typeToUse);
        }
        else
        {
            Debug.Log("Ô này đang trống hoặc đã dùng hết đồ rồi!");
        }
    }

    private void ApplyItemEffect(ItemData.ItemType type)
    {
        switch (type)
        {
            case ItemData.ItemType.Health:
                Debug.Log("Ực ực... Đã dùng LỌ HỒI MÁU!");
                // Hồi 30 máu (Cậu có thể sửa số 30 tùy ý)
                GetComponent<Health>().Heal(30f);
                break;

            case ItemData.ItemType.Speed:
                Debug.Log("Vù vù... Đã dùng THỊT TĂNG TỐC!");
                StartCoroutine(SpeedBoostCoroutine());
                break;

            case ItemData.ItemType.Shield:
                Debug.Log("Keng... Đã ăn CHÁO BẬT KHIÊN!");
                StartCoroutine(ShieldCoroutine());
                break;
        }
    }

    private IEnumerator SpeedBoostCoroutine()
    {
        Playermovement pm = GetComponent<Playermovement>();
        if (pm != null)
        {
            pm.Speed *= 1.5f; // Tăng gấp rưỡi tốc độ
            pm.currentSpeed = pm.Speed;
        }

        yield return new WaitForSeconds(5f); // Chạy nhanh trong 5 giây

        if (pm != null)
        {
            pm.Speed /= 1.5f; // Trả lại bình thường
            pm.currentSpeed = pm.Speed;
        }
    }

    private IEnumerator ShieldCoroutine()
    {
        Health health = GetComponent<Health>();

        if (health != null) health.isInvincible = true; // Bật bất tử

        yield return new WaitForSeconds(3f); // Bất tử trong 3 giây

        if (health != null) health.isInvincible = false; // Tắt bất tử
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
        if (UIManager.Instance != null) UIManager.Instance.UpdateCoinText(currentCoins);
    }

    public bool TryBuyItem(ItemData itemData)
    {
        if (SpendCoins(itemData.cost))
        {
            inventory[itemData.type]++;
            int slotIndex = GetSlotIndex(itemData.type);

            if (UIManager.Instance != null)
                UIManager.Instance.UpdateInventorySlot(slotIndex, itemData.itemIcon, inventory[itemData.type]);

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
            case ItemData.ItemType.Health: return 0;
            case ItemData.ItemType.Speed: return 1;
            case ItemData.ItemType.Shield: return 2;
            default: return 3;
        }
    }

    private ItemData.ItemType GetItemTypeBySlot(int slot)
    {
        switch (slot)
        {
            case 0: return ItemData.ItemType.Health;
            case 1: return ItemData.ItemType.Speed;
            case 2: return ItemData.ItemType.Shield;
            default: return ItemData.ItemType.Health;
        }
    }
}