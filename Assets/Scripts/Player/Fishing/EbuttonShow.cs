using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EbuttonShow : MonoBehaviour
{
    public GameObject imageEbutton;

    MainPlayer mainPlayer;
    FishingController fishingController;

    public void Start()
    {   
        mainPlayer = GetComponentInParent<MainPlayer>();
        fishingController = GetComponent<FishingController>();
        mainPlayer.ShowSpaceBar += spacebarbuttonShow;
    }

    void spacebarbuttonShow()
    {
        Collider[] hit = Physics.OverlapSphere(fishingController.center.position, fishingController.fishingRadius);
        foreach (Collider n in hit)
        {
            if (n.CompareTag("Water") && !mainPlayer.clientData.isFishing)
            {
                imageEbutton.SetActive(true);
                return;
            }
        }
        imageEbutton.SetActive(false);
    }
}
