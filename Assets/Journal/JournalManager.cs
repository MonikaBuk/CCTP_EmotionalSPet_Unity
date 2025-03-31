

using UnityEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

public class JournalManager : MonoBehaviour
{
    public static JournalManager Instance { get; private set; }
    private Dictionary<string, Dictionary<string, ActivityEntry>> journalEntries  = new Dictionary<string, Dictionary<string, ActivityEntry>>();
    static public bool newBreathingFish = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
       if (newBreathingFish)
        {
            AddBreathingExerciseReward();
            newBreathingFish = false;
        }
    }

    public void Initialize(Dictionary<string, Dictionary<string, ActivityEntry>> loadedEntries)
    {
        journalEntries = loadedEntries ?? new Dictionary<string, Dictionary<string, ActivityEntry>>();
    }


    public void AddJournalEntry(JournalEntryType entryType, string reason)
    {
        string entryTypeString = EnumHelper.GetEnumDescription(entryType); 
        AddFishReward((int)entryType, "Journal", $"{entryTypeString}: {reason}");
    }

    public void AddBreathingExerciseReward()
    {
        Debug.Log("Adding Breathing Exercise Reward"); 
        AddFishReward(5, "BreathingExercise", "Completed breathing exercise");
    }


    private void AddFishReward(int id, string activityType, string reason)
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        Debug.Log($"Adding fish reward for {activityType} on {today}");

        // Initialize today's journal entry if necessary
        if (!journalEntries.ContainsKey(today))
        {
            journalEntries[today] = new Dictionary<string, ActivityEntry>();
            Debug.Log($"Created new entry for {today}");
        }

        // Initialize the activity type if necessary
        if (!journalEntries[today].ContainsKey(activityType))
        {
            journalEntries[today][activityType] = new ActivityEntry { key = activityType };
            Debug.Log($"Created new activity type {activityType} for {today}");
        }

        int fishID = id;
        journalEntries[today][activityType].fishRewards.Add(new DailyFishReward(fishID, reason, timeStamp));
        Debug.Log($"Added fish reward: ID={fishID}, Reason={reason}, Timestamp={timeStamp}");

        // Save the updated journal data
        if (AquariumManagger.Instance != null)
        {
            Debug.Log("Saving journal data...");
            AquariumManagger.Instance.SaveJournalData(journalEntries);
            AquariumManagger.Instance.SpawnFish(fishID, reason, timeStamp);
        }
        else
        {
            Debug.LogError("AquariumManagger instance is null!");
        }
    }


    public Dictionary<string, Dictionary<string, ActivityEntry>> GetJournalData()
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
    public List<DailyFishReward> fishRewards = new List<DailyFishReward>();
}



public enum JournalEntryType
{
    [Description("Today made me smile")]
    TodayMadeMeSmile = 0,

    [Description("I am grateful for")]
    IAmGratefulFor = 1,

    [Description("I am looking forward to")]
    IAmLookingForwardTo = 2,

    [Description("My biggest achievement today")]
    MyBiggestAchievementToday = 3,

    [Description("Something new I learned")]
    SomethingNewILearned = 4
}

public static class EnumHelper
{
    public static string GetEnumDescription(Enum value)
    {
        FieldInfo fi = value.GetType().GetField(value.ToString());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }
}