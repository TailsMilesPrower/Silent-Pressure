using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BreathingUI : MonoBehaviour
{
    [Header("UI Settings")]
    public CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;

    public void FadeIn() => StartCoroutine(Fade(1f));
    public void FadeOut() => StartCoroutine(Fade(0f));

    private void Start()
    {
        if (canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.gameObject.SetActive(false);
        }
    }

    private IEnumerator Fade(float target)
    {
        float start = canvasGroup.alpha;
        float time = 0f;
        
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, target, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = target;
        canvasGroup.interactable = target > 0.9f;
        canvasGroup.blocksRaycasts = target > 0.9f;
    }
}
