using UnityEngine;

public class SokobanAudioManager : MonoBehaviour
{
    // Biến Singleton để các script khác (Player, Crate, Manager) dễ dàng gọi đến
    public static SokobanAudioManager Instance;

    [Header("Nguồn phát (Kéo thả 2 AudioSource vào đây)")]
    public AudioSource bgmSource; // Dành cho nhạc nền (cần tick Loop)
    public AudioSource sfxSource; // Dành cho hiệu ứng âm thanh

    [Header("File âm thanh (Kéo thả file .mp3 / .wav vào đây)")]
    public AudioClip bgmMusic;   // Nhạc nền lúc chơi Sokoban
    public AudioClip moveSound;  // Tiếng bước chân
    public AudioClip pushSound;  // Tiếng đẩy thùng
    public AudioClip goalSound;  // Tiếng thùng vào đích (Ting!)
    public AudioClip winSound;   // Tiếng thắng game
    public AudioClip resetSound; // Tiếng khi bấm nút R hoặc Reset

    void Awake()
    {
        // Đảm bảo chỉ có 1 AudioManager tồn tại trong scene này
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 1. TÌM VÀ TẮT NHẠC NỀN CHẠY XUYÊN MAP
        // Yêu cầu: Object giữ nhạc ở Map 1 phải được gán đúng Tag là "MapMusic"
        GameObject mapMusic = GameObject.FindGameObjectWithTag("MapMusic");
        if (mapMusic != null)
        {
            AudioSource mapAudioSource = mapMusic.GetComponent<AudioSource>();
            if (mapAudioSource != null)
            {
                mapAudioSource.Stop(); // Tắt nhạc map ngoài
            }
        }

        // 2. BẬT NHẠC NỀN CỦA MINIGAME SOKOBAN
        if (bgmMusic != null && bgmSource != null)
        {
            bgmSource.clip = bgmMusic;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    // 3. HÀM PHÁT HIỆU ỨNG ÂM THANH (Dùng chung cho mọi hành động)
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            // PlayOneShot giúp các âm thanh đè lên nhau một cách tự nhiên (không bị ngắt tiếng cũ)
            sfxSource.PlayOneShot(clip);
        }
    }
}