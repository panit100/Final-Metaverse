using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EbuttonShow : MonoBehaviour
{
    public Transform center;
    public float fishingRadius;
    public GameObject imageEbutton;

    void Update()
    {
        spacebarbuttonShow();
    }

    void spacebarbuttonShow()
    {
        Collider[] hit = Physics.OverlapSphere(center.position, fishingRadius);
        foreach (Collider n in hit)
        {
            if (n.CompareTag("Water"))
            {
                imageEbutton.SetActive(true);
                Debug.Log("Tag : Water");
            }
            else
            {
                imageEbutton.SetActive(false);
                Debug.Log("Unknow");
            }
        }
    }

}
