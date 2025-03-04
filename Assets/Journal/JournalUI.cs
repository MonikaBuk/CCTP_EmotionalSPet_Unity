using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField journalInputField;  
    [SerializeField] private Button submitButton;  

    private void Start()
    {
        submitButton.onClick.AddListener(SubmitJournalEntry);
    }

    private void SubmitJournalEntry()
    {
        string userText = journalInputField.text.Trim();  

        if (!string.IsNullOrEmpty(userText))
        {
            JournalManager.Instance.AddJournalEntry(userText);
            journalInputField.text = "";  
            enabled = false;
        }
        else
        {
            Debug.Log("Journal entry cannot be empty!");
        }
    }
}
