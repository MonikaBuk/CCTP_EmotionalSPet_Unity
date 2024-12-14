using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFishSaveData", menuName = "Fish/FishSaveData", order = 1)]

[System.Serializable]
public class FishSaveData : ScriptableObject
{
    public string achivmentName;
}

