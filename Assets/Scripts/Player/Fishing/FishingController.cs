using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingController : MonoBehaviour
{
    public Transform center;
    public float fishingRadius;
    // public bool isFishing = false;
    // Start is called before the first frame update
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
