using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DecorationIcons : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private Image bckPanel;

    private Button myButton;

    private DecorationData myDecData;

    // Start is called before the first frame update
    private void Start()
    {
        myButton = GetComponent<Button>();

        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClick);
        }
        RefreshIcon();
    }

    private void OnButtonClick()
    {
        // Iterate through the placeHolders in aquariumManagger
        if (AquariumManagger.Instance.placeHolders != null)
        {
            foreach (var placeholder in AquariumManagger.Instance.placeHolders)
            {
                if (placeholder.ID == myDecData.placeID)
                {
                    placeholder.ApplyDecoration(myDecData);
                    AquariumManagger.Instance.SetDecorationForPlaceholder(placeholder.ID, myDecData);

                }
            }
        }
    }
    public void SetUpIcon(DecorationData decor)
    {
        if (decor != null)
        {
            myDecData = decor;
            icon.sprite = decor.decorSprite;
            itemName.text = decor.itemName;
        }

    }

    public void RefreshIcon()
    {
        if (myButton != null)
        {
            if (PlayerStats.IsDecorationOwned(myDecData))
            {
                bckPanel.color = Color.green;
                myButton.interactable = true;
            }
            else
            {
                bckPanel.color = Color.grey;
                myButton.interactable = false;
            }
        }

    }

}
