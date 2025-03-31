using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DecorationIcons : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text remainingCount;
    [SerializeField] private Image bckPanel;
    public DecorationData decorationData;
    private GameObject draggedObject;
    private Button myButton;
    private bool canBePlace = true;
    public static bool itemIsChanged = false;

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

    private void Update()
    {
        if (itemIsChanged)
        {
            Debug.Log("this is true");
            RefreshIcon();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (myDecData != null && canBePlace)
        {
            draggedObject = Instantiate(AquariumManagger.Instance.baseDecoration);
            draggedObject.GetComponent<Decoration>().Initialize(myDecData);
            draggedObject.GetComponent<Decoration>().ownButton = this;

        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedObject != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            draggedObject.transform.position = worldPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedObject != null)
        {
            if (IsInsideAquarium(draggedObject.transform.position))
            {
                AquariumManagger.Instance.SaveDecorations();
                RefreshIcon();
            }
            else
            {
                Destroy(draggedObject);
            }
        }
    }
    private bool IsInsideAquarium(Vector2 position)
    {
        return position.y < -1f;
    }

    private void OnButtonClick()
    {
        myDecData = decorationData;
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
    public void MarkIconAsChanged()
    {
        itemIsChanged = true;
    }

    public void RefreshIcon()
    {
        if (myButton == null || myDecData == null)
        {
            Debug.LogWarning("RefreshIcon skipped: myButton or myDecData is null.");
            return;
        }
 
        int ownedCount = PlayerStats.GetOwnedDecorationCount(myDecData);
        int placedCount = PlayerStats.CountDecorationsInScene(myDecData);

        int itemsLeft = ownedCount - placedCount;
        Debug.Log("items left" + itemsLeft);
        remainingCount.text = itemsLeft.ToString();

        if (itemsLeft <= 0)
        {
            myButton.interactable = false;
            bckPanel.color = Color.grey;
            canBePlace = false;
           
        }
        else
        {
            bckPanel.color = Color.green;
            myButton.interactable = true;
            canBePlace = true;
        }

        // Make sure data is reset
        itemIsChanged = false;

    }

}
