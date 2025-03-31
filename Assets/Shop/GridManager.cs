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

        // Clear existing items
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        // Populate grid with all decoration items
        foreach (var item in AllItemHolder.instance.DecorationData)
        {
            GameObject gridItem = Instantiate(gridItemPrefab, gridContainer);

            var icon = gridItem.GetComponent<DecorationIcons>();
            if (icon != null)
            {
                icon.SetUpIcon(item);
            }
            else
            {
                Debug.LogWarning("DecorationIcons component missing on grid item prefab.");
            }
        }
    }
}

