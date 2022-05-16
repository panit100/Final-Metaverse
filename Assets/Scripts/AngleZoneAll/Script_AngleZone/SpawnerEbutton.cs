using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class SpawnerEbutton : NetworkBehaviour
{
    public GameObject imageEbutton;
    GameObject imageEbuttonLable;


    private void Update()
    {
        SetPlayerEbuttonimage();
    }

    public override void OnNetworkSpawn()
    {

        GameObject canvas = GameObject.FindWithTag("Canvas");
        imageEbuttonLable = Instantiate(imageEbutton, Vector3.zero, Quaternion.identity);
        imageEbuttonLable.transform.SetParent(canvas.transform);
        imageEbuttonLable.SetActive(false);

    }

    void SetPlayerEbuttonimage()
    {
        Vector3 nameLabelPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2f, 0));
        imageEbuttonLable.transform.position = nameLabelPos;
    }

    void OnTriggerStay(Collider target)
    {
        if (target.tag == "water") { imageEbuttonLable.SetActive(true); }
    }

    void OnTriggerExit(Collider target)
    {
        if (target.tag == "water") { imageEbuttonLable.SetActive(false); }
    }


}
