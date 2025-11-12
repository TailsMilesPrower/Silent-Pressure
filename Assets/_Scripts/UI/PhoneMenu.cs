using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PhoneMenu : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup phoneCanvasGroup;
    public GameObject appGrid;
    public GameObject notesPanel;
    public GameObject musicPanel;
    public GameObject emailPanel;

    [Header("Animation Settings")]
    public float fadeDuration = 0.3f;
    public Vector3 hiddenScale = new Vector3(0.8f, 0.8f, 0.8f);
    public Vector3 shownScale = Vector3.one;

    private bool isOpen = false;
    private bool isAnimating = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (phoneCanvasGroup)
        {
            phoneCanvasGroup.alpha = 0f;
            phoneCanvasGroup.interactable = false;
            phoneCanvasGroup.blocksRaycasts = false;
        }

        HideAllPanels();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame && !isAnimating)
        {
            if (isOpen)
            {
                StartCoroutine(HidePhone());
            }
            else
            {
                StartCoroutine(ShowPhone());
            }
        }
    }

    private IEnumerator ShowPhone()
    {
        isAnimating = true;
        isOpen = true;
        phoneCanvasGroup.gameObject.SetActive(true);
        
        float t = 0f;
        phoneCanvasGroup.interactable = true;
        phoneCanvasGroup.blocksRaycasts = true;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float p = t / fadeDuration;
            phoneCanvasGroup.alpha = Mathf.Lerp(0f, 1f, p);
            phoneCanvasGroup.transform.localScale = Vector3.Lerp(hiddenScale, shownScale, p);
            yield return null;
        }

        phoneCanvasGroup.alpha = 1f;
        phoneCanvasGroup.transform.localScale = shownScale;
        isAnimating = false;
    }

    private IEnumerator HidePhone()
    {
        isAnimating = true;
        isOpen = false;

        float t = 0f;
        phoneCanvasGroup.interactable = false;
        phoneCanvasGroup.blocksRaycasts = false;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float p = t / fadeDuration;
            phoneCanvasGroup.alpha = Mathf.Lerp(1f, 0f, p);
            phoneCanvasGroup.transform.localScale = Vector3.Lerp(shownScale, hiddenScale, p);
            yield return null;
        }

        phoneCanvasGroup.alpha = 0f;
        phoneCanvasGroup.transform.localScale = hiddenScale;
        phoneCanvasGroup.gameObject.SetActive(false);
        isAnimating = false;
    }

    public void OpenNotes()
    {
        HideAllPanels();
        notesPanel.SetActive(true);
    }

    public void OpenMusic()
    {
        HideAllPanels();
        musicPanel.SetActive(true);
    }

    public void OpenEmails()
    {
        HideAllPanels();
        emailPanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        notesPanel?.SetActive(false);
        musicPanel?.SetActive(false);
        emailPanel?.SetActive(false);
    }

}
