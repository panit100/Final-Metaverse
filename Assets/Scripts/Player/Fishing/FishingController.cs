using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FishingController : MonoBehaviour
{
    public FishingRodScriptableObject fishingRod;

    GameFishing_main gameFishing_Main;
    public Transform center;
    public float fishingRadius;

    public Animator animator;

    public event Action HandleFishing = delegate { };

    void Start()
    {
        GetComponentInParent<MainPlayer>().Fishing += Fishing;
        gameFishing_Main = GetComponentInParent<MainPlayer>().gameObject.GetComponentInChildren<GameFishing_main>();
    }

    private void Update() 
    {
        fishingRod = GetComponentInParent<MainPlayer>().clientData.fishingRod;
    }

    void Fishing()
    {
        Collider[] hit = Physics.OverlapSphere(center.position, fishingRadius);
        foreach (Collider n in hit)
        {
            if(n.CompareTag("Water"))
            {
                animator.SetBool("isFishing", true);
                RandomFish();
                HandleFishing();
                return;
            }
        }
        return;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center.position, fishingRadius);
    }

    void RandomFish()
    {
        int RandomwaitFish = UnityEngine.Random.Range(fishingRod.fishingChangeMin, fishingRod.fishingChangeMax);
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
