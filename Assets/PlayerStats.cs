using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private static int playerMoney = 20;
    public static Dictionary<DecorationData, int> ownedDecorations = new Dictionary<DecorationData, int>();
    public static bool decDataChanged;

    public static Dictionary<DecorationData, int> GetOwnedDecorations()
    {
        return ownedDecorations;
    }


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

        if (ownedDecorations.ContainsKey(decoration) && ownedDecorations[decoration] >= decoration.maxItemNum)
        {
            Debug.Log($"Cannot buy more than {decoration.maxItemNum} of {decoration.itemName}.");
            return false;
        }

        if (ownedDecorations.ContainsKey(decoration))
            ownedDecorations[decoration]++;
        else
            ownedDecorations[decoration] = 1;

        DecreaseMoney(decoration.cost);
        Debug.Log($"Bought {decoration.itemName}. Now owns: {ownedDecorations[decoration]}");
        return true;
    }

    public static int GetOwnedDecorationCount(DecorationData decoration)
    {
        return ownedDecorations.ContainsKey(decoration) ? ownedDecorations[decoration] : 0;
    }

    public static bool IsDecorationOwned(DecorationData decoration)
    {
        return ownedDecorations.ContainsKey(decoration) && ownedDecorations[decoration] > 0;
    }
    public static bool UseDecoration(DecorationData decoration)
    {
        if (ownedDecorations.ContainsKey(decoration) && ownedDecorations[decoration] > 0)
        {
            ownedDecorations[decoration]--;
            return true;
        }
        return false;
    }
    public static void RemoveDecoration(DecorationData decoration)
    {
       /* if (ownedDecorations.ContainsKey(decoration))
        {
            ownedDecorations[decoration]++;

            if (ownedDecorations[decoration] <= 0)
            {
                ownedDecorations.Remove(decoration);
            }
        }
       */
    }

    public static PlayerData GetPlayerData()
    {
        return new PlayerData
        {
            playerMoney = playerMoney,
            ownedDecorationNames = ownedDecorations.Keys.Select(d => d.itemName).ToList(),
            ownedDecorationCounts = ownedDecorations.Values.ToList()
        };
    }

    // Load data into PlayerStats
    public static void SetPlayerData(PlayerData data)
    {
        if (data != null)
        {
            playerMoney = data.playerMoney;
            ownedDecorations = new Dictionary<DecorationData, int>();

            List<DecorationData> decorations = AllItemHolder.GetDecorationsByNames(data.ownedDecorationNames);

            for (int i = 0; i < decorations.Count; i++)
            {
                ownedDecorations[decorations[i]] = data.ownedDecorationCounts[i];
            }

            Debug.Log($"Loaded {ownedDecorations.Count} owned decorations.");
        }
    }
    public static int CountDecorationsInScene(DecorationData data)
    {
        Decoration[] allDecorations = GameObject.FindObjectsOfType<Decoration>();
        return allDecorations.Count(d => d.data == data);
    }


}


public class PlayerData
{
    public int playerMoney; 
    public List<string> ownedDecorationNames;   
    public List<int> ownedDecorationCounts;     
}
