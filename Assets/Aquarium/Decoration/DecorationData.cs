using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDecorationData", menuName = "Aquarium/NewDecorationData", order = 1)]

[System.Serializable]
public class DecorationData : ScriptableObject
{
    public int placeID;
    public int cost;
    public Sprite decorSprite;
    public string itemName;
    public int maxItemNum;
    public float scaleMultiplier;
}
