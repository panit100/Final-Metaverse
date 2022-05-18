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

    //public GameObject Ebutton;
    //public event Action PlayerFishing = delegate { };

    public GameObject PreferGameFishing;
    GameObject PreferGameFishingLable;

    public Transform center;
    public float fishingRadius;
    //public bool isFishing = false;
    // Start is called before the first frame update
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
        Debug.Log(RandomwaitFish);
        playerMove.PlayerFishing();
        StartCoroutine(CreateGamefishing(RandomwaitFish));

    }


    IEnumerator CreateGamefishing(int RandomwaitFish)
    {
        yield return new WaitForSeconds(RandomwaitFish);
        PreferGameFishingLable = Instantiate(PreferGameFishing, Vector3.zero, Quaternion.identity, GameObject.Find("/CanvasMain").transform);
        GameFishing_main Playermoveingamefishing = PreferGameFishingLable.GetComponent<GameFishing_main>();
        Playermoveingamefishing.playermove = playerMove;

    }






}
