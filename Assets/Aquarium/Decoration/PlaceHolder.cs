using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlaceholderObject : MonoBehaviour
{
    public int ID;
    public string OwnDecorationName { get; private set; }

    public void SetDecoration(string decorationName)
    {
        OwnDecorationName = decorationName;
    }

    public void ApplyDecoration(DecorationData decoration)
    {
        if (decoration != null)
        {
            GetComponent<SpriteRenderer>().sprite = decoration.decorSprite;
            OwnDecorationName = decoration.itemName;
        }
    }
}
