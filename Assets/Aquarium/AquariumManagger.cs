using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumManagger : MonoBehaviour
{
    [SerializeField] private List<FishData> fishData;
    public List<int> decoration = new List<int>();
    public List<DecorationData> allDecorations;
    public GameObject baseFish;
    public GameObject baseDecoration;
    public Transform decorParent;
    private const string SaveKey = "AquariumSave";
    public static AquariumManagger Instance { get; private set; }

    public Camera cam;

    private static Dictionary<int, DecorationData> decorationsInScene = new Dictionary<int, DecorationData>();
    private const string JournalSaveKey = "JournalEntries";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        LoadDecorations();
        LoadJournalData();


    }
    public void SaveJournalData(Dictionary<string, Dictionary<string, ActivityEntry>> journalEntries)
    {
        JournalSaveData saveData = new JournalSaveData();

        foreach (var dailyEntry in journalEntries)
        {
            JournalEntry journalEntry = new JournalEntry { key = dailyEntry.Key };

            foreach (var activity in dailyEntry.Value)
            {
                ActivityEntry activityEntry = new ActivityEntry { key = activity.Key };
                activityEntry.fishRewards = activity.Value.fishRewards;

                journalEntry.activities.Add(activityEntry);
            }

            saveData.journalEntries.Add(journalEntry);
        }
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(JournalSaveKey, json);
        PlayerPrefs.Save();

        Debug.Log("Journal data saved.");
    }


    public void LoadJournalData()
    {
        if (!PlayerPrefs.HasKey(JournalSaveKey)) return;

        string json = PlayerPrefs.GetString(JournalSaveKey);
        Debug.Log("Load fish: " + json);

        JournalSaveData loadedData = JsonUtility.FromJson<JournalSaveData>(json);

        var journalEntries = new Dictionary<string, Dictionary<string, ActivityEntry>>();
        foreach (var journalEntry in loadedData.journalEntries)
        {
            var dailyDictionary = new Dictionary<string, ActivityEntry>();

            foreach (var activity in journalEntry.activities)
            {
                dailyDictionary[activity.key] = activity;
            }

            journalEntries[journalEntry.key] = dailyDictionary;
        }

        JournalManager.Instance.Initialize(journalEntries);
        float fishCounter = 0;
        foreach (var dailyEntry in journalEntries)
        {
            foreach (var activity in dailyEntry.Value.Values)
            {
                foreach (var fishReward in activity.fishRewards)

                {
                    Debug.Log("Fish ID to spawn: " + fishReward.reason);
                    SpawnFish(fishReward.fishID, fishReward.dateTime, fishReward.reason);
                    fishCounter++;
                }

            }
        }
        if (fishCounter < 10)
        {
            cam.orthographicSize = 3;
        }
        else if (fishCounter < 20)
        {
            cam.orthographicSize = 3.5f;
        }
        else if (fishCounter < 30)
        {
            cam.orthographicSize = 4;
        }
        else
        {
            cam.orthographicSize = 4.5f;
        }

    }
    public void SpawnFish(int fishID, string dateTime, string reason)
    {
        Vector3 randomPos = new Vector3(UnityEngine.Random.Range(-3, 3), UnityEngine.Random.Range(-3, 3), 0);
        GameObject newFish = Instantiate(baseFish, randomPos, Quaternion.identity);
        FishComponent fishComponent = newFish.GetComponent<FishComponent>();
        fishComponent.Initialize(fishID, dateTime, reason);
        SpriteRenderer spriteRenderer = newFish.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            if (fishID >= 0 && fishID < fishData.Count)
            {
                spriteRenderer.sprite = fishData[fishID].fishSprite;
            }
            else
            {
                Debug.LogError($"Invalid fishID: {fishID}. It must be between 0 and {fishData.Count - 1}.");
            }
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the new fish GameObject.");
        }
    }
    public static void LoadAquarium()
    {
        string json = PlayerPrefs.GetString(SaveKey);
        Debug.Log($"Loading aquarium data: {json}");

        if (!string.IsNullOrEmpty(json))
        {
            AquariumSaveData loadedData = JsonUtility.FromJson<AquariumSaveData>(json);
            if (loadedData != null)
            {

                if (loadedData.decorationsInSceneS != null)
                {
                    Debug.Log("Decorations loaded: Not null");
                    Debug.Log($"Decoration count in scene: {loadedData.decorationsInSceneS.Count}");

                    decorationsInScene.Clear();

                    foreach (var entry in loadedData.decorationsInSceneS)
                    {
                        if (!string.IsNullOrEmpty(entry.DecorationName))
                        {
                            Debug.Log($"Trying to find decoration for: {entry.DecorationName}");

                            if (AllItemHolder.instance == null)
                            {
                                Debug.LogError("Instance or AllItemHolder is null!");
                            }
                            else
                            {
                                DecorationData decoration = AllItemHolder.instance.DecorationData.Find(d => d.itemName == entry.DecorationName);

                                if (decoration != null)
                                {
                                    decorationsInScene[entry.ID] = decoration;
                                    Debug.Log($"Decoration found and added for ID: {entry.ID}");
                                }
                                else
                                {
                                    Debug.LogWarning($"Decoration not found for name: {entry.DecorationName}");
                                    decorationsInScene[entry.ID] = null;
                                }
                            }
                        }
                        else
                        {
                            decorationsInScene[entry.ID] = null;
                            Debug.LogWarning($"DecorationName is empty or null for ID: {entry.ID}");
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Loaded data: Decorations list is null.");
                }
            }
            else
            {
                Debug.LogError("Loaded aquarium data is null!");
            }
        }
        else
        {
            Debug.LogWarning("No aquarium data found in PlayerPrefs.");
        }
    }

    public void SaveDecorations()
    {
        AquariumSaveData savedAquaObjects = new AquariumSaveData();

        foreach (var decorObj in FindObjectsOfType<Decoration>())
        {
            DecorationSaveEntry saveEntry = new DecorationSaveEntry
            {
                ID = decorObj.GetInstanceID(),
                DecorationName = decorObj.data.itemName,
                PositionX = decorObj.transform.position.x,
                PositionY = decorObj.transform.position.y
            };

            savedAquaObjects.decorationsInSceneS.Add(saveEntry);
        }

        string json = JsonUtility.ToJson(savedAquaObjects, false);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    public void SetDecorationForPlaceholder(int placeholderID, DecorationData selectedDecoration)
    {
        decorationsInScene[placeholderID] = selectedDecoration;
    }
    public void RemoveDecorationFromSave(DecorationData data, Vector2 position)
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;

        string json = PlayerPrefs.GetString(SaveKey);
        AquariumSaveData loadedData = JsonUtility.FromJson<AquariumSaveData>(json);

        loadedData.decorationsInSceneS.RemoveAll(d =>
            d.DecorationName == data.itemName && Mathf.Approximately(d.PositionX, position.x) && Mathf.Approximately(d.PositionY, position.y)
        );

        string newJson = JsonUtility.ToJson(loadedData, false);
        PlayerPrefs.SetString(SaveKey, newJson);
        PlayerPrefs.Save();
        Debug.Log($"Removed {data.itemName} from saved decorations.");
    }

    public void LoadDecorations()
    {
        if (!PlayerPrefs.HasKey(SaveKey)) return;

        string json = PlayerPrefs.GetString(SaveKey);
        AquariumSaveData loadedData = JsonUtility.FromJson<AquariumSaveData>(json);

        foreach (var entry in loadedData.decorationsInSceneS)
        {
            DecorationData decorationData = AllItemHolder.instance.DecorationData.Find(d => d.itemName == entry.DecorationName);
            if (decorationData == null) continue;

            // Instantiate decoration at saved position
            GameObject decorObj = Instantiate(baseDecoration, new Vector2(entry.PositionX, entry.PositionY), Quaternion.identity, decorParent);
            decorObj.GetComponent<Decoration>().Initialize(decorationData);
        }
    }

    private void OnApplicationQuit()
    {
        SaveAquarium();
    }

    public  void SaveAquarium()
    {
       // SaveJournalData();
        SaveDecorations();
    }


}

[System.Serializable]
public class AquariumSaveData
{
    public List<DecorationSaveEntry> decorationsInSceneS = new List<DecorationSaveEntry>();
}

[System.Serializable]
public class DecorationSaveEntry
{
    public int ID;
    public string DecorationName;
    public float PositionX;
    public float PositionY;
}
