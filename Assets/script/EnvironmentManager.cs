using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class EnvironmentManager : MonoBehaviour
{
   [Header("1. THAY ĐỔI MAP (PARALLAX CROSS-FADE)")]
    [Tooltip("Kéo Object CHA chứa các lớp map hiện tại vào đây")]
    public GameObject oldParallaxParent; 
    
    [Tooltip("Kéo Object CHA chứa các lớp map tươi sáng vào đây")]
    public GameObject newParallaxParent; 

    [Header("2. ÁNH SÁNG THEO KÈM")]
    public Light2D globalLight;
    public Color newLightColor = Color.white;
    public float newLightIntensity = 1f;

    [Header("CÀI ĐẶT THỜI GIAN")]
    public float transitionDuration = 3f;

    private SpriteRenderer[] oldLayers;
    private SpriteRenderer[] newLayers;

    public void OnBossDefeated()
    {
        StartCoroutine(FadeToNewParallax());
    }

    private IEnumerator FadeToNewParallax()
    {
        if (oldParallaxParent != null) oldLayers = oldParallaxParent.GetComponentsInChildren<SpriteRenderer>();
        
        if (newParallaxParent != null)
        {
            newParallaxParent.SetActive(true);
            newLayers = newParallaxParent.GetComponentsInChildren<SpriteRenderer>();
            
            SetAlpha(newLayers, 0f);
        }

        float elapsedTime = 0f;
        Color startLightColor = globalLight != null ? globalLight.color : Color.white;
        float startIntensity = globalLight != null ? globalLight.intensity : 1f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            if (globalLight != null)
            {
                globalLight.color = Color.Lerp(startLightColor, newLightColor, t);
                globalLight.intensity = Mathf.Lerp(startIntensity, newLightIntensity, t);
            }

            FadeAlpha(oldLayers, 1f - t);

            FadeAlpha(newLayers, t);

            yield return null; 
        }

        if (oldParallaxParent != null)
        {
            Destroy(oldParallaxParent);
        }
    }

    private void SetAlpha(SpriteRenderer[] layers, float alpha)
    {
        if (layers == null) return;
        foreach (var sr in layers)
        {
            if (sr != null)
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }
        }
    }

    private void FadeAlpha(SpriteRenderer[] layers, float alpha)
    {
        if (layers == null) return;
        foreach (var sr in layers)
        {
            if (sr != null)
            {
                Color c = sr.color;
                c.a = alpha;
                sr.color = c;
            }
        }
    }
}
