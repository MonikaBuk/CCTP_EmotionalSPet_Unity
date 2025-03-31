using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text warningText;
    public TMP_Text optOutText;
    public TMP_Text optInText;

    private bool isOptedOut = false;
    private List<ScoreEntry> pastScores = new List<ScoreEntry>();
    private string lastQuestionnaireDate;
    private string savePath;

    public GameObject questionareText;
    public GameObject notTimeText;
    public GameObject optedOutText;

    public GameObject questionareButton;
    public GameObject optInB;
    public GameObject optOutB;
    public GameObject ExpB;
    public GameObject GraphB;


    public ScoreGraph scoreGraph;

    private void Start()
    {
        SetUI();
        lastQuestionnaireDate = PlayerPrefs.GetString("LastQuestionnaireDate", "");
        if (ShouldShowQuestionnaire())
        {
            ShowQuestionnaire();
        }
        LoadData();
    }


    private void SetUI()
    {
        isOptedOut = PlayerPrefs.GetInt("OptOut", 0) == 1;

        if (isOptedOut)
        {
            optOutText.gameObject.SetActive(true);

            optOutB.gameObject.SetActive(false);
            optInB.gameObject.SetActive(true);
            questionareButton.SetActive(false);
            ExpB.SetActive(false);
            GraphB.SetActive(false);

            questionareText.gameObject.SetActive(true);
            notTimeText.gameObject.SetActive(false);
            optedOutText.gameObject.SetActive(false);

        }
        else
        {
            optInB.gameObject.SetActive(false);
            optOutB.gameObject.SetActive(true);
            optOutText.gameObject.SetActive(false);
            ExpB.SetActive(true);
            GraphB.SetActive(true);

        }
    }

    private bool ShouldShowQuestionnaire()
    {
        if (isOptedOut)
        {
            return false;
        }
        if (string.IsNullOrEmpty(lastQuestionnaireDate))
        {
            return true;
        }
        DateTime lastDate = DateTime.Parse(lastQuestionnaireDate);
        if (DateTime.UtcNow.Subtract(lastDate).Days >= 7)
        {
            return true;
        }
        questionareButton.SetActive(false);
        questionareText.gameObject.SetActive(false);
        notTimeText.gameObject.SetActive(true);
        optedOutText.gameObject.SetActive(false);

        return false;
    }

    private void ShowQuestionnaire()
    {
        questionareText.gameObject.SetActive(true);
        notTimeText.gameObject.SetActive(false);
        optedOutText.gameObject.SetActive(false);
        questionareButton.SetActive(true); 
    }


    public void OptOut()
    {
        isOptedOut = true;
        PlayerPrefs.SetInt("OptOut", 1);
        PlayerPrefs.Save();
        optOutText.gameObject.SetActive(true);
        ShowWarning("You have opted out of future questionnaires.");
        SetUI();
    }
    public void OptIn()
    {
        isOptedOut = false;
        PlayerPrefs.SetInt("OptOut", 0);
        PlayerPrefs.DeleteKey("LastQuestionnaireDate"); 

        optOutText.gameObject.SetActive(false); 
        optInText.gameObject.SetActive(true); 
        ShowWarning("You have opted back in for future questionnaires.");
        SetUI();
    }

    public void ExportGraphDataToFile()
    {
        if (pastScores.Count == 0)
        {
            ShowWarning("No scores available to export!");
            return;
        }

        string savePath = Application.persistentDataPath + "/PSS4GraphData.txt"; 
        string fileContent = "Scores:\n";  

        foreach (var score in pastScores)
        {
            fileContent += score.date + ": " + score.score + "\n";
        }
        File.WriteAllText(savePath, fileContent);
        Debug.Log("Graph data exported to: " + savePath);
        ShowWarning("Your graph data has been exported!");
    }


    private void SaveData()
    {
        string json = JsonUtility.ToJson(new SavedData { scores = pastScores }, true);
        savePath = Application.persistentDataPath + "/PSS4Scores.json";
        File.WriteAllText(savePath, json);
        Debug.Log("Scores saved to: " + savePath);
    }

    private void LoadData()
    {
        string savePath = Application.persistentDataPath + "/PSS4Scores.json";
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            var savedData = JsonUtility.FromJson<SavedData>(json);
            pastScores = savedData.scores ?? new List<ScoreEntry>(); 
        }
    }
    private void ShowWarning(string message)
    {
        if (warningText != null)
        {
            warningText.text = message;
            warningText.gameObject.SetActive(true);
        }
    }

    public void ViewGraph()
    {

        if (pastScores.Count > 0)
        {
            scoreGraph.ShowGraph(pastScores);
            ShowWarning("Your Scores.");
        }
        else
        {
            ShowWarning("No scores to show in the graph.");
        }
    }

    public void StoreScore(int score)
    {
        pastScores.Add(new ScoreEntry(score)); // Store with date
        Debug.Log("Score stored: " + score);
        PlayerPrefs.SetString("LastQuestionnaireDate", DateTime.UtcNow.ToString("yyyy-MM-dd"));
        PlayerPrefs.Save();
        SaveData();
    }

    [System.Serializable]
    private class SavedData
    {
        public List<ScoreEntry> scores = new List<ScoreEntry>();
    }

    [System.Serializable]
    public class ScoreEntry
    {
        public int score;
        public string date;

        public ScoreEntry(int score)
        {
            this.score = score;
            this.date = System.DateTime.UtcNow.ToString("yyyy-MM-dd");
        }
    }
}
