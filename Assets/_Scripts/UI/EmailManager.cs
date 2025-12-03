using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EmailManager : MonoBehaviour
{
    public static EmailManager Instance;

    public TMP_Text emailListText;

    private class Email
    {
        public string sender;
        public string title;
        public string body;
    }

    private List<Email> emails = new List<Email>();
    private static bool created = false; 

    private void Awake()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
        RefreshEmailUI();
        if (!created)
        {
            created = true;
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        /*
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        */
    }

    public void RefreshEmailUI()
    {
        if (emailListText == null) return;
    }

    /*
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RebindUI();
    }

    private void RebindUI()
    {
        emailListText = GameObject.Find("Email_Text")?.GetComponent<TMP_Text>();
        //RefreshEmailList();
    }
    */

    public void AddEmail(string sender, string title, string body)
    {
        emails.Add(new Email { sender = sender, title = title, body = body });
        RefreshEmailList();
    }

    private void RefreshEmailList()
    {
        if (!emailListText) return;

        emailListText.text = "";

        foreach (var mail in emails)
        {
            emailListText.text +=
                $"<b>From:</b> {mail.sender}\n" +
                $"<b>Subject:</b> {mail.title}\n\n" +
                $"{mail.body}\n" +
                "-----------------------------";
        }
    }
}
