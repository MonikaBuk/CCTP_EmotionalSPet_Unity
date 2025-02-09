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

    private Button myButton;

    private DecorationData myDecData;

    private void Start()
    {
        myButton = GetComponent<Button>();

        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClick);
        }
        RefreshIcon();
    }


    public void SetUpIcon(DecorationData decor)
    {
        if (decor != null)
        {
            myDecData = decor;
            icon.sprite = decor.decorSprite;
            itemName.text = decor.itemName;
            itemCost.text = decor.cost.ToString();
        }

    }

    public void OnButtonClick()
    {
        if (myDecData == null) return;

        if (PlayerStats.TryBuyDecoration(myDecData))
        {
            Debug.Log($"Purchased: {myDecData.itemName}");
            var shopItems = FindObjectOfType<ShopItems>();
            if (shopItems != null)
            {
                shopItems.RefreshShop();
            }
        }
        else
        {
            Debug.LogWarning($"Could not purchase: {myDecData.itemName}");
        }
    }


    public void RefreshIcon()
    {
        if (myDecData == null) return;

        if (PlayerStats.IsDecorationOwned(myDecData))
        {
            bckPanel.color = Color.grey;
            myButton.interactable = false;
        }
        else if (myDecData.cost <= PlayerStats.GetPlayerMoney())
        {
            bckPanel.color = myDecData.cost <= PlayerStats.GetPlayerMoney() ? Color.green : Color.red;
            myButton.interactable = true;
        }
        else
        {
            bckPanel.color = Color.red;
        }
    }


}
