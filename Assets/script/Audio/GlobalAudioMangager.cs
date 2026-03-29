using UnityEngine;

public class GlobalAudioManager : MonoBehaviour
{
    public static GlobalAudioManager Instance;

    [Header("Nguồn phát (Kéo 2 AudioSource vào đây)")]
    public AudioSource bgmSource; // Dành cho nhạc nền
    public AudioSource sfxSource; // Dành cho tiếng động

    [Header("--- NHẠC NỀN (BGM) ---")]
    public AudioClip defaultMapBGM; // Nhạc chung bình yên cho Map 1, 2, 3, 4
    public AudioClip bossFightBGM;  // Nhạc dồn dập khi đập Boss Map 4

    [Header("--- SFX: PLAYER ---")]
    public AudioClip attackMelee;   // Tiếng chém kiếm
    public AudioClip attackMagic;   // Tiếng chưởng phép từ xa
    public AudioClip playerHurt;    // Tiếng rên khi mất máu
    public AudioClip playerDie;     // Tiếng gục ngã
    public AudioClip footstep;      // Tiếng bước chân (chạy)

    [Header("--- SFX: TƯƠNG TÁC ---")]
    public AudioClip coinPickup;    // Tiếng lụm vàng (Keng!)
    public AudioClip teleport;      // Tiếng qua Scene (Vù vù)

    [Header("--- SFX: QUÁI NHỎ (MOBS) ---")]
    public AudioClip mobHurt;       // Tiếng quái nhỏ bị đánh trúng
    public AudioClip mobDie;        // Tiếng quái nhỏ chết bốc hơi
    public AudioClip mobBite;       // Tiếng quái cắn (Mob 1)
    public AudioClip mobShoot;      // Tiếng quái bắn đạn xa (Mob 2)
    public AudioClip mobSlash;      // Tiếng quái chém (Mob 3)

    [Header("--- SFX: BOSS TỔNG ---")]
    public AudioClip bossRoar;         // Tiếng gầm xuất hiện
    public AudioClip bossMelee;        // Chiêu 1: Tiếng đập cận chiến rung đất
    public AudioClip bossSpiralShoot;  // Chiêu 2: Tiếng bắn bão đạn xoắn ốc (xèo xèo/chíu chíu liên tục)
    public AudioClip bossMeteor;       // Chiêu 3: Tiếng thiên thạch rơi (vù... BÙM!)
    public AudioClip bossHurt;         // Tiếng Boss rên rỉ khi trúng đòn
    public AudioClip bossDie;          // Tiếng Boss chết nổ tung
    public AudioClip victory;          // Tiếng kèn chiến thắng

    void Awake()
    {
        // Bí quyết để nhạc đi xuyên suốt Map 1 -> 4
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Không bao giờ bị phá hủy khi qua màn
        }
        else
        {
            Destroy(gameObject); // Tránh tình trạng nhân bản 2 tổng đài
        }
    }

    void Start()
    {
        // Ngay khi bật game, phát nhạc bình yên mặc định
        PlayBGM(defaultMapBGM);
    }

    // Hàm gọi tiếng động (Lụm vàng, chém, chạy...)
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    // Hàm dùng để đổi nhạc nền (Đổi sang nhạc Boss)
    public void PlayBGM(AudioClip newBGM)
    {
        if (newBGM != null && bgmSource != null)
        {
            bgmSource.clip = newBGM;
            bgmSource.Play();
        }
    }
}