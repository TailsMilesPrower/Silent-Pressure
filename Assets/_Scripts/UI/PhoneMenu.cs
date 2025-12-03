using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

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

    private bool uiRegistered = false;
    
    private TextMeshProUGUI noteText;
    private TextMeshProUGUI emailText;

    private GameObject notificationPopup;
    private TextMeshProUGUI notificationText;
    private GameObject notificationBadge;

    private AudioSource phoneAudio;

    private void Awake()
    {
        /*
        if (phoneRoot != null)
        {
            phoneRoot.SetActive(false);
        }
        */

        isOpen = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        /*
        if (phoneCanvasGroup)
        {
            phoneCanvasGroup.alpha = 0f;
            phoneCanvasGroup.interactable = false;
            phoneCanvasGroup.blocksRaycasts = false;
        }
        */
        yield return new WaitForSeconds(0.1f);
        RegisterPhoneUI();
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
                if (NotificationManager.Instance != null) NotificationManager.Instance.HidePopupImmediatly();
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

        RegisterPhoneUI();

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

        //if (ObjectiveManager.Instance) ObjectiveManager.Instance.UpdateNotesUI();
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
        NotificationManager.Instance.OnPhoneOpened();
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

    public void RegisterPhoneUI()
    {
        
        if (uiRegistered) return;

        if (GameObject.Find("Canvas") == null) return;

        Transform phone = GameObject.Find("Canvas").transform;
        
        notesPanel = phone.Find("Phone/PhoneMenu/AppPanels/NotesPanel").gameObject;
        emailPanel = phone.Find("Phone/PhoneMenu/AppPanels/EmailPanel").gameObject;
        musicPanel = phone.Find("Phone/PhoneMenu/AppPanels/MusicPanel").gameObject;

        noteText = phone.Find("Phone/PhoneMenu/AppPanels/NotesPanel/Note_Text")?.GetComponent<TextMeshProUGUI>();
        emailText = phone.Find("Phone/PhoneMenu/Email_Box/Mail/Email_Text")?.GetComponent<TextMeshProUGUI>();

        notificationPopup = phone.Find("NotificationPopup").gameObject;
        notificationText = phone.Find("NotificationPopup/Notification_Text")?.GetComponent<TextMeshProUGUI>();

        notificationBadge = phone.Find("Phone/PhoneMenu/AppGrid/EmailsButton/Notification_Badge")?.gameObject;

        phoneAudio = phone.GetComponent<AudioSource>();
        if (phoneAudio == null) phoneAudio = phone.gameObject.AddComponent<AudioSource>();

        if (PlayerController.Instance != null)
        {
            StressMeter stressMeter = PlayerController.Instance.GetComponent<StressMeter>();
            if (stressMeter != null)
            {
                Transform canvas = GameObject.Find("Canvas").transform;
                stressMeter.needle = canvas.Find("Needle")?.GetComponent<RectTransform>();
            }
        }

        uiRegistered = true;

        /* //old way
        if (ObjectiveManager.Instance != null)
        {
            ObjectiveManager.Instance.notesText = transform.Find("AppPanels/NotesPanel/Note_Text")?.GetComponent<TMP_Text>();
        }

        if (EmailManager.Instance != null)
        {
            EmailManager.Instance.emailListText = transform.Find("Email_Box/Mail/Email_Text")?.GetComponent<TMP_Text>();
        }

        if (NotificationManager.Instance != null)
        {
            NotificationManager.Instance.popupCanvasGroup = transform.Find("NotificationPopup")?.GetComponent<CanvasGroup>();
            NotificationManager.Instance.popupText = transform.Find("Notification_Text")?.GetComponent<TMP_Text>();
            NotificationManager.Instance.notificationBadge = transform.Find("AppGrid/EmailsButton/Notification_Badge").gameObject;
            NotificationManager.Instance.audioSource = transform.Find("Audio Source")?.GetComponent<AudioSource>();
        }
        */
    }

}
