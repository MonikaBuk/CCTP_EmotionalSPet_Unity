using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopIcon : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text itemCost;
    [SerializeField] private Image bckPanel;

    public void SetUpIcon(DecorationData decor)
    {
        if (decor != null)
        {
            icon.sprite = decor.decorSprite;
            itemName.text = decor.itemName;
            itemCost.text = decor.cost.ToString();

            if (decor.cost <= PlayerStats.GetPlayerMoney())
            {
                bckPanel.color = Color.green; 
            }
            else
            {
                bckPanel.color = Color.red; 
            }
        }

    }
}
