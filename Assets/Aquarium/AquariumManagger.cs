using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumManagger : MonoBehaviour
{
    [SerializeField] private List<FishData> fishData;
   // private AllItemHolder allItemHolder;

    [SerializeField] public List<PlaceholderObject> placeHolders; 
    private static List<string> fishes = new List<string>();
    public  List<int> decoration = new List<int>();
    public GameObject baseFish;
    private const string SaveKey = "AquariumSave";
    public static AquariumManagger Instance { get; private set; }

    private static Dictionary<int, DecorationData> decorationsInScene = new Dictionary<int, DecorationData>();

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

        LoadFishes();
        LoadDecoration();
        if (decorationsInScene.Count != placeHolders.Count)
        {
            LoadAquarium();
            foreach (var placeholder in placeHolders)
            {
                DecorationData tempDec = null;
                if (decorationsInScene.TryGetValue(placeholder.ID, out tempDec) && tempDec != null)
                {
                    placeholder.ApplyDecoration(tempDec);
                }
                else
                {
                    decorationsInScene[placeholder.ID] = null;

                }

            }
        }

       // decorationData = allItemHolder.DecorationData;
    }

    public static void LoadAquarium()
    {
        string json = PlayerPrefs.GetString(SaveKey);
        Debug.Log($"Loading aquarium data: {json}");  // Debug log to check the raw JSON string

        if (!string.IsNullOrEmpty(json))
        {
            AquariumSaveData loadedData = JsonUtility.FromJson<AquariumSaveData>(json);
            Debug.Log($"Loaded data: Fishes count = {loadedData.fishes.Count}, Decorations count = {loadedData.decorationsInSceneS.Count}");
            if (loadedData != null)
            {
                fishes = loadedData.fishes;

                // Check if decorationsInSceneS is valid before processing
                if (loadedData.decorationsInSceneS != null)
                {
                    Debug.Log("Decorations loaded: Not null");
                    Debug.Log($"Decoration count in scene: {loadedData.decorationsInSceneS.Count}");

                    // Clear the previous decorations in the scene
                    decorationsInScene.Clear();

                    // Loop through each decoration entry in the loaded data
                    foreach (var entry in loadedData.decorationsInSceneS)
                    {
                        // Check if DecorationName is valid and not empty
                        if (!string.IsNullOrEmpty(entry.DecorationName))
                        {
                            Debug.Log($"Trying to find decoration for: {entry.DecorationName}");

                            // Check if the Instance and allItemHolder are available
                            if (AllItemHolder.instance == null)
                            {
                                Debug.LogError("Instance or AllItemHolder is null!");
                            }
                            else
                            {
                                // Find the decoration by name in allItemHolder's DecorationData
                                DecorationData decoration = AllItemHolder.instance.DecorationData.Find(d => d.itemName == entry.DecorationName);

                                // Check if the decoration was found
                                if (decoration != null)
                                {
                                    decorationsInScene[entry.ID] = decoration;
                                    Debug.Log($"Decoration found and added for ID: {entry.ID}");
                                }
                                else
                                {
                                    Debug.LogWarning($"Decoration not found for name: {entry.DecorationName}");
                                    decorationsInScene[entry.ID] = null;  // Set to null if decoration is not found
                                }
                            }
                        }
                        else
                        {
                            decorationsInScene[entry.ID] = null;  // Handle case where DecorationName is empty or null
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



    public static void SaveAquarium()
    {
        AquariumSaveData savedAquaObjects = new AquariumSaveData();
        savedAquaObjects.fishes = fishes;


        foreach ((int id, DecorationData data) in decorationsInScene)
        {
            Debug.Log($"current objects in  {decorationsInScene.Count}");
            DecorationSaveEntry saveEntry = new DecorationSaveEntry
            {
                ID = id,
                DecorationName = data?.itemName // Save the itemName, or null if data is null
            };

            savedAquaObjects.decorationsInSceneS.Add(saveEntry);
            Debug.Log($"current to save objects in  {savedAquaObjects.decorationsInSceneS.Count}");
        }


        string json = JsonUtility.ToJson(savedAquaObjects, false);
        Debug.Log(json);

        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }



    public void SetDecorationForPlaceholder(int placeholderID , DecorationData selectedDecoration)
    {
        decorationsInScene[placeholderID] = selectedDecoration;
    }


    public static void AddNewFish(string newFish)
    {
        fishes.Add(newFish);
        SaveAquarium();
    }

    public void LoadFishes()
    {
        foreach (string savedData in fishes)
        {
            FishData data = fishData.Find(fish => fish.fishName.Equals(savedData));
            GameObject newFish = Instantiate(baseFish, this.transform);
            newFish.GetComponent<SpriteRenderer>().sprite = data.fishSprite;
            newFish.transform.localScale = baseFish.transform.localScale;
        }
    }
    
    public void LoadDecoration()
    {
        foreach (var placeholder in placeHolders)
        {
            if (decorationsInScene.TryGetValue(placeholder.ID, out DecorationData decoration) && decoration != null)
            {
                // Apply the decoration to the placeholder
                placeholder.ApplyDecoration(decoration);
            }
            else
            {
                // If no decoration is found for the placeholder, reset it or leave it empty
                placeholder.ApplyDecoration(null); // Optionally handle no decoration state
                Debug.LogWarning($"No decoration found for placeholder ID: {placeholder.ID}");
            }
        }
    }
    private void OnApplicationQuit()
    {
        SaveAquarium();
    }

}

[System.Serializable]
public class AquariumSaveData
{
    public List<string> fishes = new List<string>();
    public List<DecorationSaveEntry> decorationsInSceneS = new List<DecorationSaveEntry>();
}

[System.Serializable]
public class DecorationSaveEntry
{
    public int ID;
    public string DecorationName; // Nullable or null for no decoration
}