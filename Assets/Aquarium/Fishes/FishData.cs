using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFishData", menuName = "Fish/FishData", order = 1)]

[System.Serializable]
public class FishData : ScriptableObject
{
    public string fishName;
    public Sprite fishSprite;
}


