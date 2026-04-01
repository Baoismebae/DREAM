using UnityEngine;
using System.Collections;

public class PortalController : MonoBehaviour
{
    [Header("HIỆU ỨNG XUẤT HIỆN")]
    public float appearDuration = 1.5f;

    [Header("GIAO DIỆN KẾT THÚC")]
    public GameObject endScreenPanel; 
    public CanvasGroup endScreenCanvasGroup; 

    private Vector3 targetScale;
    private bool isPlayerEntered = false;

    void Awake()
    {
        targetScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(ScaleUpPortal());
    }

    IEnumerator ScaleUpPortal()
    {
        float elapsedTime = 0f;
        while (elapsedTime < appearDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / appearDuration;
            
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            
            transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
            yield return null;
        }
        transform.localScale = targetScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPlayerEntered)
        {
            isPlayerEntered = true;
            StartCoroutine(EndGameRoutine());
        }
    }

    IEnumerator EndGameRoutine()
    {
        if (endScreenPanel != null)
        {
            endScreenPanel.SetActive(true);

            if (endScreenCanvasGroup != null)
            {
                endScreenCanvasGroup.alpha = 0f;
                float elapsed = 0f;
                float fadeTime = 2f; 

                while (elapsed < fadeTime)
                {
                    elapsed += Time.deltaTime;
                    endScreenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeTime);
                    yield return null;
                }
                endScreenCanvasGroup.alpha = 1f;
            }
        }

        Time.timeScale = 0f;
    }
}
