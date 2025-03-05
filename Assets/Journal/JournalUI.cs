using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class JournalUI : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown journalDropdown;
    [SerializeField] private TMP_InputField journalInputField;  
    [SerializeField] private Button submitButton;  

    private void Start()
    {
        submitButton.onClick.AddListener(SubmitJournalEntry);
        PopulateDropdown();
    }

    private void PopulateDropdown()
    {
        journalDropdown.ClearOptions();
        List<string> options = new List<string>();

        foreach (JournalEntryType type in Enum.GetValues(typeof(JournalEntryType)))
        {
            options.Add(EnumHelper.GetEnumDescription(type)); // Get readable description
        }

        journalDropdown.AddOptions(options);
    }

    private void SubmitJournalEntry()
    {
        string userText = journalInputField.text.Trim();

        if (!string.IsNullOrEmpty(userText))
        {
            JournalEntryType selectedType = (JournalEntryType)journalDropdown.value; 
            JournalManager.Instance.AddJournalEntry(selectedType, userText);

            journalInputField.text = "";
            enabled = false;
        }
        else
        {
            Debug.Log("Journal entry cannot be empty!");
        }
    }
}
