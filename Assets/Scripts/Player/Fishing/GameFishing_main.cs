using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class GameFishing_main : MonoBehaviour
{
    public MainPlayer mainPlayer;
    public Image BarFishing_Image;

    private void Start() {
        mainPlayer = GetComponentInParent<MainPlayer>();    
    }

    void Update()
    {
        BarFishingdown_Image();
        if (Input.GetKeyDown(KeyCode.Space)) { BarFishingSpacebarUp_Image();  }

    }

    void BarFishingdown_Image()
    {
        BarFishing_Image.fillAmount -= Time.deltaTime * 0.05f;
        if (BarFishing_Image.fillAmount == 0)
        {
            Debug.Log("Fail Fishing");
            FinishFishing();
        }

    }

    void BarFishingSpacebarUp_Image()
    {
        BarFishing_Image.fillAmount += 0.05f;
        if (BarFishing_Image.fillAmount == 1)
        {
            Debug.Log("Win Fishing");
            FinishFishing();
        }

    }

    void FinishFishing()
    {
        mainPlayer.clientData.isFishing = false;
        gameObject.SetActive(false);
    }


}
