using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private static int playerMoney = 20;
    private static List<DecorationData> ownedDecorations = new List<DecorationData>();

    public static void AddMoney(int money)
    {
        playerMoney += money;
    }

    public static int GetPlayerMoney()
    {
        return playerMoney;
    }

    public static void DecreaseMoney(int money)
    {
        playerMoney -= money;
    }

    public static bool TryBuyDecoration(DecorationData decoration)
    {
        if (playerMoney < decoration.cost)
        {
            Debug.Log("Not enough money!");
            return false;
        }

        if (ownedDecorations.Contains(decoration))
        {
            Debug.Log($"Player already owns {decoration.itemName}.");
        }
        else
        {
            ownedDecorations.Add(decoration);
        }

        DecreaseMoney(decoration.cost);
        Debug.Log($"Bought {decoration.itemName}");
        return true;
    }

    public static List<DecorationData> GetOwnedDecorations()
    {
        return ownedDecorations;
    }

    public static bool IsDecorationOwned(DecorationData decoration)
    {
        return ownedDecorations.Contains(decoration);
    }

    // Save data for PlayerStats
    public static PlayerData GetPlayerData()
    {
        return new PlayerData
        {
            playerMoney = playerMoney,
            ownedDecorationNames = ownedDecorations.Select(d => d.itemName).ToList()
        };
    }

    // Load data into PlayerStats
    public static void SetPlayerData(PlayerData data)
    {
        if (data != null)
        {
            playerMoney = data.playerMoney;

            // Convert saved decoration names back to ScriptableObject references
            ownedDecorations = AllItemHolder.GetDecorationsByNames(data.ownedDecorationNames);

            // Log the loaded decorations
    
            foreach (var decoration in ownedDecorations)
            {
                Debug.Log($"Decoration: {decoration.itemName}");
            }

            Debug.Log($"Number of Owned Decorations: {ownedDecorations.Count}");
        }
    }

}


    public class PlayerData
{
    public int playerMoney; // Player's current amount of money
    public List<string> ownedDecorationNames; // List of owned decoration names (unique identifiers)
}
