using UnityEngine;
using System;

public class SokobanManager : MonoBehaviour
{
    // Tạo một sự kiện tĩnh (static event) để các script khác dễ dàng lắng nghe
    public static Action OnMinigameWon;

    [Header("Cài đặt Sokoban")]
    public int totalTargets; // Tổng số điểm đích trong phòng
    private int currentFilledTargets = 0; // Số điểm đích đang có thùng

    // Gọi hàm này khi một thùng được đẩy vào đích
    public void TargetFilled()
    {
        currentFilledTargets++;
        CheckWinCondition();
    }

    // Gọi hàm này nếu thùng bị đẩy ra khỏi đích
    public void TargetEmptied()
    {
        currentFilledTargets--;
    }

    private void CheckWinCondition()
    {
        if (currentFilledTargets >= totalTargets)
        {
            Debug.Log("Sokoban hoàn thành! Phát tín hiệu mở cửa...");
            // Kích hoạt sự kiện chiến thắng
            OnMinigameWon?.Invoke();
        }
    }
}