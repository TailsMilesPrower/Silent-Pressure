using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip notificationSound;
    public AudioClip vibrationSound;

    [Header("UI Pop-up")]
    public CanvasGroup popupCanvasGroup;
    public TMP_Text popupText;
    public float fadeDuration = 0.4f;
    public float visibleTime = 100000f;

    [Header("Badge")]
    public GameObject notificationBadge;

    private bool isShowing = false;
    private bool hasUnreadNotification = false;

    private void Awake()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    /*
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindUI();
    }

    private void RebindUI()
    {
        popupCanvasGroup = GameObject.Find("NotificationPopup")?.GetComponent<CanvasGroup>();
        popupText = GameObject.Find("Notification_Text")?.GetComponent<TMP_Text>();

        notificationBadge = GameObject.Find("Notification_Badge");

        audioSource = GameObject.Find("Audio Source")?.GetComponent<AudioSource>();
    }
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (notificationBadge) notificationBadge.SetActive(false);

        if (popupCanvasGroup)
        {
            popupCanvasGroup.alpha = 0f;
            popupCanvasGroup.gameObject.SetActive(false);
        }
    }

    public void TriggerNotification(string message = "Press TAB")
    {
        //string objective = ObjectiveManager.Instance.GetObjective();
        //string email = $"New Objective Received:\n\n{objective}";

        //EmailManager.Instance.AddEmail(email);
        //string key = ObjectiveManager.Instance.GetObjectiveKey();

        string key = ObjectiveManager.Instance != null ? ObjectiveManager.Instance.GetObjectiveKey() : "system";
        if (NotificationManager.Instance != null) SendEmailForObjective(key);
        NotificationBadgeShow();

        StartCoroutine(NotificationRoutine(message));
    }

    private IEnumerator NotificationRoutine(string message)
    {
        if (isShowing) yield break;

        isShowing = true;
        
        if (audioSource)
        {
            if (notificationSound) audioSource.PlayOneShot(notificationSound);
            if (vibrationSound) audioSource.PlayOneShot(vibrationSound, 0.7f);
        }

        popupText.text = message;
        popupCanvasGroup.gameObject.SetActive(true);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            popupCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        popupCanvasGroup.alpha = 1f;
        yield return new WaitForSeconds(visibleTime);

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            popupCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        popupCanvasGroup.alpha = 0f;
        popupCanvasGroup.gameObject.SetActive(false);

        isShowing = false;
    }

    private void SendEmailForObjective(string key)
    {
        if (EmailManager.Instance == null) return;
        
        switch (key)
        {
            case "train":
                EmailManager.Instance.AddEmail(
                    "Mom",
                    "Family Dinner Tonight",
                    "Honey, we are gathering for dinner with auntie Ginny tonight.\n" +
                    "You are always welcome to join us.\n\nLove, Mom"
                );
                break;

            case "supermarket":
                EmailManager.Instance.AddEmail(
                    "AI Assistant - Tom",
                    "Shopping Reminder",
                    "The online shopping app is currently under maintenance.\n" +
                    "Don't forget to get your groceries at the supermarket!\n" +
                    "Tip: Buy something healthy today."
                );
                break;

            case "therapist":
                EmailManager.Instance.AddEmail(
                    "Dr. Levin",
                    "Therapy Appointment",
                    "Just a gentle reminder about your ongoing therapy sessions.\n" +
                    "Feel free to stop by if you need to talk today.\n\n" +
                    "Take care, \nDr. Levin"
                );
                break;
            
            case "home":
                EmailManager.Instance.AddEmail(
                    "System",
                    "Return Home",
                    "You've completed your tasks for today.\n" +
                    "Head back home and rest."
                );
                break;
                /*
            case "library":
                EmailManager.Instance.AddEmail(
                    "City Library",
                    "Book Service",
                    "The books you requested to arrive are here.\n" +
                    "You can get them as of today!\n\n" +
                    "Thank you for supporting your local library.\n" +
                    "And don't forget to return the last books you took!"
                );
                break;
                */

            default:
                EmailManager.Instance.AddEmail(
                    "System",
                    "New Objective",
                    ObjectiveManager.Instance != null ?
                    ObjectiveManager.Instance.GetObjective() : "New objective"
                ); 
                break;
        }
    }

    public void NotificationBadgeShow()
    {
        notificationBadge.SetActive(true);
        hasUnreadNotification = true;
    }

    public void NotificationBadgeHide()
    {
        notificationBadge.SetActive(false);
        hasUnreadNotification = false;
    }

    public void OnPhoneOpened()
    {
        if (hasUnreadNotification) NotificationBadgeHide();
    }

    public void HidePopupImmediatly()
    {
        if (popupCanvasGroup)
        {
            StopAllCoroutines();
            popupCanvasGroup.alpha = 0f;
            popupCanvasGroup.gameObject.SetActive(false);
            isShowing = false;
        }
    }
}
