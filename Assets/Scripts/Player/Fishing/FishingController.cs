using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

[System.Serializable]
public class FishingRodData
{
    public string name;
    [Range(0f,100f)]
    public float dropRate = 0;

    [Range(0f,100f)]
    public float fishingChange = 0;

}

public class FishingController : MonoBehaviour
{
    public FishingRodData fishingRod;
    public PlayerMove playerMove;

    public GameFishing_main gameFishing_Main;
    public Transform center;
    public float fishingRadius;

    void Start()
    {
        GetComponentInParent<MainPlayer>().Fishing += Fishing;
    }


    void Fishing()
    {
        Collider[] hit = Physics.OverlapSphere(center.position, fishingRadius);
        foreach (Collider n in hit)
        {
            if(n.CompareTag("Water"))
            { 
                RandomFish();
            }
            else { return; }
        }

    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center.position, fishingRadius);
    }

    void RandomFish()
    {
        int RandomwaitFish = Random.Range(1, 10);
        Debug.Log("Wait Fish");
        StartCoroutine(CreateGamefishing(RandomwaitFish));

    }

    IEnumerator CreateGamefishing(int RandomwaitFish)
    {
        yield return new WaitForSeconds(RandomwaitFish);
        gameFishing_Main.BarFishing_Image.fillAmount = 0.5f;
        gameFishing_Main.gameObject.SetActive(true);

    }






}
