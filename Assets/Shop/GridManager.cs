using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Transform gridContainer;
    public GameObject gridItemPrefab;
    private void Start()
    {
        PopulateGrid();
        
    }

    public void PopulateGrid()
    {
        if (AllItemHolder.instance == null || AllItemHolder.instance.DecorationData == null)
        {
            Debug.LogError("AllItemHolder or DecorationData list is not set.");
            return;
        }
        Dictionary<int, List<DecorationData>> groupedItems = new Dictionary<int, List<DecorationData>>();
        foreach (var decoration in AllItemHolder.instance.DecorationData)
        {
            if (!groupedItems.ContainsKey(decoration.placeID))
            {
                groupedItems[decoration.placeID] = new List<DecorationData>();
            }
            groupedItems[decoration.placeID].Add(decoration);
        }
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (var group in groupedItems)
        {
            GameObject rowContainer = new GameObject($"Row_{group.Key}");
            rowContainer.transform.SetParent(gridContainer);

            HorizontalLayoutGroup rowLayoutGroup = rowContainer.AddComponent<HorizontalLayoutGroup>();
            rowLayoutGroup.childAlignment = TextAnchor.UpperLeft; 
            rowLayoutGroup.spacing = 10;  
            rowLayoutGroup.childScaleHeight = true;
            rowLayoutGroup.childScaleWidth = true;
            rowLayoutGroup.childControlHeight = false;
            rowLayoutGroup.childControlWidth = false;
            rowLayoutGroup.childForceExpandHeight = false;
            rowLayoutGroup.childForceExpandWidth = false;

            foreach (var item in group.Value)
            {
                GameObject gridItem = Instantiate(gridItemPrefab, rowContainer.transform);
 
                var icon = gridItem.GetComponent<DecorationIcons>();
                if (icon != null)
                {
                    icon.SetUpIcon(item); 
                }
                else
                {
                    Debug.LogWarning("DecorationIcons component missing on grid item prefab.");
                }
                RectTransform gridItemRect = gridItem.GetComponent<RectTransform>();
                if (gridItemRect != null)
                {
                    gridItemRect.sizeDelta = new Vector2(150, 150);  
                }
            }   
        }
    }
}

