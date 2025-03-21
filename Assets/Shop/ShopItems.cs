using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShopItems : MonoBehaviour
{
    [SerializeField] public List<DecorationData> allDecoration;

    [SerializeField] private GameObject shopIconPrefab; // Reference to the ShopIcon prefab
    [SerializeField] private Transform gridContainer; // Reference to the grid container (panel)
    [SerializeField] private TMP_Text moneyAmmount;

    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        moneyAmmount.text = PlayerStats.GetPlayerMoney().ToString();
        // Clear existing children (if necessary)
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        // Loop through each decoration and create an icon
        foreach (var decor in allDecoration)
        {
            // Instantiate the prefab
            GameObject iconInstance = Instantiate(shopIconPrefab, gridContainer);

            // Set up the ShopIcon component
            ShopIcon shopIcon = iconInstance.GetComponent<ShopIcon>();
            if (shopIcon != null)
            {
                shopIcon.SetUpIcon(decor);
            }
        }
    }
    public void RefreshShop()
    {
        PopulateShop();
    }
}
