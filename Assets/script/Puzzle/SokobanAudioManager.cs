using UnityEngine;

public class SokobanAudioManager : MonoBehaviour
{
    public static SokobanAudioManager Instance;

    [Header("Nguồn phát âm thanh (Kéo thả 2 AudioSource vào đây)")]
    public AudioSource bgmSource; // Phát nhạc nền
    public AudioSource sfxSource; // Phát hiệu ứng (bíp, rẹt rẹt...)

    [Header("File âm thanh (Kéo file .mp3 / .wav vào đây)")]
    public AudioClip bgmMusic; // Nhạc nền minigame
    public AudioClip moveSound; // Tiếng bước chân
    public AudioClip pushSound; // Tiếng đẩy thùng
    public AudioClip goalSound; // Tiếng thùng vào đích (Ting!)
    public AudioClip winSound; // Tiếng thắng game (Tada!)
    public AudioClip resetSound; // Tiếng reset

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Khi mở scene Sokoban, bật nhạc nền ngay lập tức
        if (bgmMusic != null && bgmSource != null)
        {
            bgmSource.clip = bgmMusic;
            bgmSource.loop = true;
            bgmSource.Play();
        }

        // TẮT NHẠC MAP 3 CŨ (Nếu nó lỡ chạy qua scene này)
        // Tìm object chứa nhạc Map 3 (giả sử bạn đặt tên tag là "Map3Music" hoặc tên là "BGM_Map3")
        // Nếu bạn bị hai nhạc đè nhau, hãy báo tôi để tôi viết thêm dòng tắt nhạc Map 3 nhé.
    }

    // Hàm dùng chung để phát bất kỳ hiệu ứng nào
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}