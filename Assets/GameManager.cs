using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public  TMP_Text lol;

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load data on start
       
        //Instance.lol.color = Color.green;
    }
    private void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        LoadPlayerData();
      //  AquariumManagger.LoadAquarium();

    }

    public static void SaveGame()
    {
        SavePlayerData();
        //AquariumManagger.SaveAquarium();
    }
    public static void SavePlayerData()
    {
        // Create a PlayerData instance
        PlayerData playerData = new PlayerData
        {
            playerMoney = PlayerStats.GetPlayerMoney(),

            // Convert the dictionary into lists for decoration names and counts
            ownedDecorationNames = PlayerStats.GetOwnedDecorations().Keys.Select(d => d.itemName).ToList(),
            ownedDecorationCounts = PlayerStats.GetOwnedDecorations().Values.ToList()
        };

        // Serialize PlayerData to JSON
        string json = JsonUtility.ToJson(playerData, false);

        // Save JSON to PlayerPrefs
        PlayerPrefs.SetString("PlayerSaveData", json);
        PlayerPrefs.Save();

        Debug.Log("Player data saved: " + json);
    }

    public static void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey("PlayerSaveData"))
        {
            // Retrieve JSON from PlayerPrefs
            string json = PlayerPrefs.GetString("PlayerSaveData");
           

            // Deserialize JSON into PlayerData
            PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);

            // Log the loaded player data
            Debug.Log("Loaded Player Data:");
            Debug.Log($"Player Money: {playerData.playerMoney}");
            Debug.Log("Owned Decoration Names: " + string.Join(", ", playerData.ownedDecorationNames));

            // Load data into PlayerStats
            PlayerStats.SetPlayerData(playerData);

            Debug.Log("Player data loaded.");
        }
        else
        {
            Debug.Log("No save data found.");
        }
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}


