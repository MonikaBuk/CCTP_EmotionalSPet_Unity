using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class PSS4Test : MonoBehaviour
{
    public ToggleGroup question1Group;
    public ToggleGroup question2Group;
    public ToggleGroup question3Group;
    public ToggleGroup question4Group;
    public TMP_Text warningText;
    public TMP_Text scoreText;
    public GameObject scorePanel;
    public UIManager manager;

    public void CalculateScore()
    {
        int score = 0;

        score += GetSelectedValue(question1Group);
        score += GetSelectedValue(question2Group);
        score += GetSelectedValue(question3Group);
        score += GetSelectedValue(question4Group);

        Debug.Log("PSS-4 Total Score: " + score);
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }
        scorePanel.gameObject.SetActive(true);
        scoreText.text = "PSS-4 Total Score: " + score;
        manager.StoreScore(score);
    }

    public void OnDone()
    {
        if (AreAllQuestionsAnswered())
        {
            CalculateScore();
        }
        else
        {
            ShowWarning("Please answer all questions before submitting!");
        }
    }

    private int GetSelectedValue(ToggleGroup toggleGroup)
    {
        foreach (var toggle in toggleGroup.ActiveToggles())
        {
            ToggleValue toggleValue = toggle.GetComponent<ToggleValue>();
            if (toggleValue != null)
            {
                return toggleValue.value;
            }
        }
        return 0;
    }

    private bool AreAllQuestionsAnswered()
    {
        return IsAnswered(question1Group) &&
               IsAnswered(question2Group) &&
               IsAnswered(question3Group) &&
               IsAnswered(question4Group);
    }
    private bool IsAnswered(ToggleGroup toggleGroup)
    {
        return toggleGroup.ActiveToggles().Any(); 
    }

   
    private void ShowWarning(string message)
    {
        if (warningText != null)
        {
            warningText.text = message;
            warningText.gameObject.SetActive(true);
        }
    }
}