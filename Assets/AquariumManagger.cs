using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AquariumManagger : MonoBehaviour
{
    [SerializeField] private List<FishData> fishData;
    private static List<string> fishes = new List<string>();
    private static List<string> decoration = new List<string>();
    public GameObject baseFish;
    private const string SaveKey = "AquariumSave";

    void Start()
    {
        LoadFishes();
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