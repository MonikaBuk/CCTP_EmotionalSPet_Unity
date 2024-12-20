using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private static int playerMoney = 5;
    private static List<DecorationData> ownedDecorations = new List<DecorationData>();


    public static void AddMoney(int money)
    {
        playerMoney += money;

        // Refresh the shop UI
        var shopItems = FindObjectOfType<ShopItems>();
        if (shopItems != null)
        {
            shopItems.RefreshShop();
        }
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

        // Check if the player already owns this decoration
        if (ownedDecorations.Contains(decoration))
        {
            Debug.Log($"Player already owns {decoration.itemName}. Replacing...");
        }
        else
        {
            ownedDecorations.Add(decoration);
        }

        // Deduct money
        DecreaseMoney(decoration.cost);

        Debug.Log($"Bought {decoration.itemName}");
        return true;
    }
    public static List<DecorationData> GetOwnedDecorations()
    {
        return new List<DecorationData>(ownedDecorations);
    }
    public static bool IsDecorationOwned(DecorationData decoration)
    {
        return ownedDecorations.Contains(decoration);
    }
}
