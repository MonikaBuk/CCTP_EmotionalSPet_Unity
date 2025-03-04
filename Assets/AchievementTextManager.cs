using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AchievementTextManager : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text text;

    public void ActivateText(string newText)
    {
        panel.SetActive(true);
        text.text = newText;
    }

    public void DeactivateText()
    {
        panel.SetActive(false);
    }
}


