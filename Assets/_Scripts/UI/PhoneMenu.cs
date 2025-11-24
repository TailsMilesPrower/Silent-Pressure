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

    [Header("Player Control Reference")]
    public PlayerController playerController;

    private bool isOpen = false;
    private bool isAnimating = false;

    [Header("Music System Reference")]
    public ListenToMusic musicSystem;

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
        SetCursorLocked(true);
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

        if (playerController) playerController.enabled = false;
        SetCursorLocked(false);

        phoneCanvasGroup.gameObject.SetActive(true);
        phoneCanvasGroup.interactable = true;
        phoneCanvasGroup.blocksRaycasts = true;

        float t = 0f;
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

        if (playerController) playerController.enabled = true;
        SetCursorLocked(true);

        phoneCanvasGroup.interactable = false;
        phoneCanvasGroup.blocksRaycasts = false;

        float t = 0f;
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

        if (ObjectiveManager.Instance) ObjectiveManager.Instance.UpdateNotesUI();
    }

    public void OpenMusic()
    {
        HideAllPanels();
        musicPanel.SetActive(true);
    }

    public void PlaySong1() { if (musicSystem) musicSystem.PlaySong(1); }
    public void PlaySong2() { if (musicSystem) musicSystem.PlaySong(2); }
    public void PlaySong3() { if (musicSystem) musicSystem.PlaySong(3); }

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

    private void SetCursorLocked(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState= CursorLockMode.None;
            Cursor.visible = true;
        }    

    }


}
