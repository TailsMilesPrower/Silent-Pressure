using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

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
    public float visibleTime = 5f;

    [Header("Badge")]
    public GameObject notificationBadge;

    private bool isShowing = false;
    private bool hasUnreadNotification = false;

    private void Awake()
    {
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

    public void TriggerNotification(string message = "Press TAB to open your phone")
    {
        //string objective = ObjectiveManager.Instance.GetObjective();
        //string email = $"New Objective Received:\n\n{objective}";

        //EmailManager.Instance.AddEmail(email);

        string key = ObjectiveManager.Instance.GetObjectiveKey();
        SendEmailForObjective(key);
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

            /*
            case "therapist":
                EmailManager.Instance.AddEmail(
                    "Dr. Levin",
                    "Therapy Appointment",
                    "Just a gentle reminder about your ongoing therapy sessions.\n" +
                    "Feel free to stop by if you need to talk today.\n\n" +
                    "Take care, \nDr. Levin"
                );
                break;
            */

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

            default:
                EmailManager.Instance.AddEmail(
                    "System",
                    "New Objective",
                    ObjectiveManager.Instance.GetObjective()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
