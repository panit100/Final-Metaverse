using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class GachaData
{
    public FishingRodScriptableObject rod;
    [Range(0f,100f)]
    public float dropRateMin = 0;
    [Range(0f,100f)]
    public float dropRateMax = 0;
}

public class FishingRodGacha : MonoBehaviour
{
    public List<GachaData> gachaDatas = new List<GachaData>();
    // Start is called before the first frame update
    void Start()
    {
        MainPlayer[] mainPlayer = FindObjectsOfType<MainPlayer>();
        foreach(MainPlayer n in mainPlayer)
        {
            n.PlayGacha += GachaRandom;
        }
    }

    void GachaRandom(ClientData client)
    {
        float random = UnityEngine.Random.Range(0f,100f);

        for(int i = 0;i< gachaDatas.Count;i++)
        {
            if(random >= gachaDatas[i].dropRateMin && random < gachaDatas[i].dropRateMax)
            {
                client.fishingRod = gachaDatas[i].rod;
            }
        }
    }
}
