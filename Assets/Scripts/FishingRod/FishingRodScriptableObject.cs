using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewFishingRod", menuName = "FishingRod/CreateFishingRod")]
public class FishingRodScriptableObject : ScriptableObject
{
    public new string name;
    public Sprite image;
    public int rank;
    
    [Range(0f,100f)]
    public int fishingChangeMin = 0;
    [Range(0f,100f)]
    public int fishingChangeMax = 0;
    public int gainFish = 1;

}
