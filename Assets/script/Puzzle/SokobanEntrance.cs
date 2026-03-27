using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Cần thiết để dùng Coroutine (IEnumerator)

public class SokobanEntrance : MonoBehaviour
{
    [Header("Scene & Cửa")]
    public GameObject door;
    public string minigameSceneName = "SokobanScene";

    [Header("UI & Âm thanh")]
    public GameObject pressEText; // Kéo Text "Press E" vào đây
    public AudioClip interactSound; // Kéo file âm thanh (.mp3, .wav) vào đây

    private bool isPlayerInZone = false;
    private AudioSource audioSource;

    void Start()
    {
        // Lấy hoặc tự động thêm AudioSource để phát nhạc
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Đảm bảo Text bị ẩn khi mới vào game
        if (pressEText != null) pressEText.SetActive(false);

        // Kiểm tra trạng thái thắng game
        if (GameManager.isSokobanSolved)
        {
            if (door != null) door.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(KeyCode.E) && !GameManager.isSokobanSolved)
        {
            // Bắt đầu quá trình chuyển scene có âm thanh
            StartCoroutine(TransitionToMinigame());
        }
    }

    IEnumerator TransitionToMinigame()
    {
        // Ẩn chữ đi ngay lập tức khi đã bấm E
        if (pressEText != null) pressEText.SetActive(false);

        // Phát âm thanh nếu có
        if (interactSound != null)
        {
            audioSource.PlayOneShot(interactSound);
            // Đợi 1 giây để nghe tiếng xong rồi mới load Scene (bạn có thể chỉnh số này)
            yield return new WaitForSeconds(1f);
        }

        SceneManager.LoadScene(minigameSceneName);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if (pressEText != null) pressEText.SetActive(true); // Hiện chữ lên
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if (pressEText != null) pressEText.SetActive(false); // Ẩn chữ đi
        }
    }
}