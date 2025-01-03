using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumManagger : MonoBehaviour
{
    [SerializeField] private List<FishData> fishData;
    [SerializeField] private AllItemHolder allItemHolder;
   // [SerializeField] public List<DecorationData> decorationData;
    [SerializeField] public List<PlaceholderObject> placeHolders; 
    private static List<string> fishes = new List<string>();
    public  List<int> decoration = new List<int>();
    public GameObject baseFish;
    private const string SaveKey = "AquariumSave";
    public static AquariumManagger Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
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
       // decorationData = allItemHolder.DecorationData;
    }

    public static void LoadAquarium()
    {
        string json = PlayerPrefs.GetString(SaveKey);
        if (!string.IsNullOrEmpty(json))
        {
            AquariumSaveData loadedData = JsonUtility.FromJson<AquariumSaveData>(json);
            fishes = loadedData.fishes;
        }
    }


    public static void SaveAquarium()
    {
        AquariumSaveData savedAquaObjects = new AquariumSaveData();
        savedAquaObjects.fishes = fishes;
        string json = JsonUtility.ToJson(savedAquaObjects, false);
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();

    }

    public void SetDecorationForAllPlaceholders(DecorationData selectedDecoration)
    {
        foreach (var placeholder in placeHolders)
        {
            placeholder.SetDecoration(selectedDecoration.itemName);
            ///placeholder.LoadDecoration(decorationData); // Apply decoration if it's found
        }
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
        }
    }

    public void LoadDecoration()
    {
       /* foreach (int savedData in decoration)
        {
            DecorationData data = decorationData.Find(decoration => decoration.placeID == savedData);
            foreach(PlaceholderObject placeHolder in placeHolders)
            {
                if (placeHolder.ID == data.placeID)
                {
                    placeHolder.gameObject.GetComponent<SpriteRenderer>().sprite = data.decorSprite;
                }
            }
        } */
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
    public List<string> decoration = new List<string>();
}