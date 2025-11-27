using UnityEngine;
using TMPro;
using System.Collections.Generic;

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

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
