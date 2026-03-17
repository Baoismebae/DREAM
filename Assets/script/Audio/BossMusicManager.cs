using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioSource chillSource;
    public AudioSource bossSource;
    public float fadeDuration = 1.5f; // Thời gian chuyển nhạc (giây)

    private bool isBossMode = false;

    void Update()
    {
        // Kiểm tra nếu ấn phím E và chưa vào chế độ Boss
        if (Input.GetKeyDown(KeyCode.E) && !isBossMode)
        {
            StartBossBattle();
        }
    }

    void StartBossBattle()
    {
        isBossMode = true;

        // Bắt đầu phát nhạc Boss (nhưng volume vẫn đang là 0)
        bossSource.Play();

        // Chạy Coroutine để xử lý chuyển âm lượng mượt mà
        StartCoroutine(CrossFadeMusic());

        Debug.Log("Boss xuất hiện! Nhạc thay đổi!");
    }

    IEnumerator CrossFadeMusic()
    {
        float timeElapsed = 0;

        while (timeElapsed < fadeDuration)
        {
            // Tỉ lệ phần trăm thời gian đã trôi qua
            float lerpValue = timeElapsed / fadeDuration;

            // Chill giảm dần từ 1 -> 0, Boss tăng dần từ 0 -> 1
            chillSource.volume = Mathf.Lerp(1f, 0f, lerpValue);
            bossSource.volume = Mathf.Lerp(0f, 1f, lerpValue);

            timeElapsed += Time.deltaTime;
            yield return null; // Chờ đến frame tiếp theo
        }

        // Đảm bảo âm lượng chuẩn xác sau khi kết thúc loop
        chillSource.volume = 0;
        bossSource.volume = 1;
        chillSource.Stop(); // Tắt hẳn nhạc chill để tiết kiệm tài nguyên
    }
}