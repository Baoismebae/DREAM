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

    [Header("HIỆU ỨNG HẠT (VFX)")]
    public GameObject healFX;   // Kéo object Aura xanh vào đây
    public GameObject speedFX;  // Kéo object Trail xanh lam vào đây
    public GameObject shieldFX; // Kéo object Hào quang trắng vào đây

    void Awake()
    {
        // Thần chú giữ dữ liệu xuyên không gian
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; // Tránh lỗi chạy đè code
        }

        // Khởi tạo túi đồ
        inventory[ItemData.ItemType.Health] = 0;
        inventory[ItemData.ItemType.Speed] = 0;
        inventory[ItemData.ItemType.Shield] = 0;
    }

    void Start()
    {
        UpdateCoinUI();

        // Đảm bảo tắt hết các hiệu ứng lúc mới vào game
        if (healFX != null) healFX.SetActive(false);
        if (speedFX != null) speedFX.SetActive(false);
        if (shieldFX != null) shieldFX.SetActive(false);
    }

    void Update()
    {
        // LẮNG NGHE PHÍM CÁCH (SPACE) ĐỂ DÙNG ĐỒ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseItem();
        }
    }

    private void UseItem()
    {
        if (UIManager.Instance == null) return;

        int selectedSlot = UIManager.Instance.GetSelectedSlot();
        ItemData.ItemType type = GetItemTypeBySlot(selectedSlot);

        // Nếu trong túi đang có món đồ thuộc ô đang chọn
        if (inventory.ContainsKey(type) && inventory[type] > 0)
        {
            inventory[type]--; // Trừ 1 món đi
            UIManager.Instance.ConsumeItemUI(selectedSlot, inventory[type]); // Báo UIManager trừ số

            ApplyEffect(type); // Kích hoạt phép thuật

            // Phát âm thanh uống/nhai
            if (GlobalAudioManager.Instance != null && GlobalAudioManager.Instance.useItemSound != null)
            {
                GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.useItemSound);
            }
        }
        else
        {
            Debug.Log("Ô này đang trống hoặc đã dùng hết đồ!");
        }
    }

    private void ApplyEffect(ItemData.ItemType type)
    {
        switch (type)
        {
            case ItemData.ItemType.Health:
                Health h = GetComponent<Health>();
                if (h != null) h.Heal(30f); // Cộng máu
                StartCoroutine(TriggerVFX(healFX, 1.5f));
                break;

            case ItemData.ItemType.Speed:
                StartCoroutine(SpeedRoutine()); // Tăng tốc
                break;

            case ItemData.ItemType.Shield:
                StartCoroutine(ShieldRoutine()); // Bật khiên
                break;
        }
    }

    // ==========================================
    // CÁC COROUTINE ĐIỀU KHIỂN THỜI GIAN & HIỆU ỨNG
    // ==========================================

    // Hiệu ứng hồi máu (Chỉ nháy lên 1.5s rồi tắt)
    private IEnumerator TriggerVFX(GameObject vfx, float time)
    {
        if (vfx != null)
        {
            vfx.SetActive(false); // Tắt trước để reset
            vfx.SetActive(true);  // Bật lên

            // Ép hệ thống hạt phải xịt ra (Sửa lỗi tàng hình)
            ParticleSystem ps = vfx.GetComponent<ParticleSystem>();
            if (ps != null) ps.Play();

            yield return new WaitForSeconds(time);
            vfx.SetActive(false); // Hết thời gian thì tắt đi
        }
    }

    // Ăn Thịt chạy nhanh trong 5s
    private IEnumerator SpeedRoutine()
    {
        Playermovement pm = GetComponent<Playermovement>();
        if (pm != null)
        {
            if (speedFX != null)
            {
                speedFX.SetActive(false);
                speedFX.SetActive(true);
                ParticleSystem ps = speedFX.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play(); // Ép xịt hạt
            }

            pm.currentSpeed = pm.Speed * 1.8f; // Tăng gần gấp đôi tốc độ

            yield return new WaitForSeconds(5f); // Đếm ngược 5 giây

            pm.currentSpeed = pm.Speed; // Hết 5s trả lại tốc độ cũ
            if (speedFX != null) speedFX.SetActive(false);
        }
    }

    // Uống cháo bật Khiên bất tử trong 3s
    private IEnumerator ShieldRoutine()
    {
        Health h = GetComponent<Health>();
        if (h != null)
        {
            if (shieldFX != null)
            {
                shieldFX.SetActive(false);
                shieldFX.SetActive(true);
                ParticleSystem ps = shieldFX.GetComponent<ParticleSystem>();
                if (ps != null) ps.Play(); // Ép xịt hạt
            }

            h.isInvincible = true; // Bật chế độ bất tử chặn sát thương

            yield return new WaitForSeconds(3f); // Đếm ngược 3 giây

            h.isInvincible = false; // Tắt bất tử
            if (shieldFX != null) shieldFX.SetActive(false);
        }
    }

    // ==========================================
    // CÁC HÀM QUẢN LÝ TIỀN VÀ MUA HÀNG
    // ==========================================

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

    public bool TryBuyItem(ItemData data)
    {
        if (currentCoins >= data.cost)
        {
            currentCoins -= data.cost;
            inventory[data.type]++; // Bỏ đồ vào túi

            UpdateCoinUI(); // Trừ tiền trên UI

            // Cập nhật số lượng đồ lên 4 ô giao diện
            int slotIndex = GetSlotIndex(data.type);
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdateInventorySlot(slotIndex, data.itemIcon, inventory[data.type]);
            }

            // Gọi tiếng Ting Ting mua thành công
            if (GlobalAudioManager.Instance != null && GlobalAudioManager.Instance.buySuccessSound != null)
            {
                GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.buySuccessSound);
            }

            return true;
        }
        else
        {
            // Gọi tiếng Bíp Bíp hết tiền
            if (GlobalAudioManager.Instance != null && GlobalAudioManager.Instance.buyFailSound != null)
            {
                GlobalAudioManager.Instance.PlaySFX(GlobalAudioManager.Instance.buyFailSound);
            }

            return false;
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
}