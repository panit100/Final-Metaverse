using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Transform center;
    public float fishingRadius;

    void Start()
    {
        GetComponentInParent<MainPlayer>().Fishing += Fishing;
    }

    void Fishing()
    {
        Collider[] hit = Physics.OverlapSphere(center.position,fishingRadius);
        foreach(Collider n in hit)
        {
            if(n.CompareTag("Water"))
            {
                // isFishing = true;
                Debug.Log("Fishing");
            }
            else
            {
                return;
            }
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center.position, fishingRadius);
    }
}
