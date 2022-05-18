using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EbuttonShow : MonoBehaviour
{
    bool FishingRun = false;

    public Transform center;
    public float fishingRadius;
    public GameObject imageEbutton;

    public void Start()
    {   GetComponentInParent<MainPlayer>().CheckShowSpacebar += CheckShowSpacebar;  }


    void Update()
    {
        spacebarbuttonShow();
    }

    void spacebarbuttonShow()
    {
        Collider[] hit = Physics.OverlapSphere(center.position, fishingRadius);
        foreach (Collider n in hit)
        {
            if (n.CompareTag("Water") && FishingRun != true)
            {
                imageEbutton.SetActive(true);
                return;
            }
        }
        imageEbutton.SetActive(false);
    }

    void CheckShowSpacebar()
    {
        FishingRun = true;
    }

}
