

using UnityEngine;
using System;
using System.Collections.Generic;

public class JournalManager : MonoBehaviour
{
    public static JournalManager Instance { get; private set; }
    private Dictionary<string, Dictionary<string, DailyFishReward>> journalEntries = new Dictionary<string, Dictionary<string, DailyFishReward>>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Initialize(Dictionary<string, Dictionary<string, DailyFishReward>> loadedEntries)
    {
        journalEntries = loadedEntries ?? new Dictionary<string, Dictionary<string, DailyFishReward>>();
    }

    public void AddJournalEntry(string reason)
    {
        AddFishReward("Journal", reason);
    }

    public void AddBreathingExerciseReward()
    {
        AddFishReward("BreathingExercise", "Completed breathing exercise");
    }

    private void AddFishReward(string activityType, string reason)
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        if (!journalEntries.ContainsKey(today))
        {
            journalEntries[today] = new Dictionary<string, DailyFishReward>();
        }

        if (!journalEntries[today].ContainsKey(activityType))
        {
            int fishID = UnityEngine.Random.Range(0, 3); // Pick a random fish
            journalEntries[today][activityType] = new DailyFishReward(fishID, reason, timeStamp);

            AquariumManagger.Instance.SaveJournalData(journalEntries);
        }
        else
        {
            Debug.Log($"You already received a fish for {activityType} today!");
        }
    }

    public DailyFishReward GetJournalEntry(string date, string activityType)
    {
        return journalEntries.ContainsKey(date) && journalEntries[date].ContainsKey(activityType)
            ? journalEntries[date][activityType]
            : null;
    }

    public Dictionary<string, Dictionary<string, DailyFishReward>> GetJournalData()
    {
        return journalEntries;
    }
}
[System.Serializable]
public class DailyFishReward
{
    public int fishID;
    public string reason;
    public string dateTime;

    public DailyFishReward(int fishID, string reason, string dateTime)
    {
        this.fishID = fishID;
        this.reason = reason;
        this.dateTime = dateTime;
    }
}


[System.Serializable]
public class JournalSaveData
{
    public List<JournalEntry> journalEntries = new List<JournalEntry>();
}

[System.Serializable]
public class JournalEntry
{
    public string key;
    public List<ActivityEntry> activities = new List<ActivityEntry>();
}

[System.Serializable]
public class ActivityEntry
{
    public string key;
    public DailyFishReward fishReward;
}
