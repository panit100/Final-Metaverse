using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;

public class GameFishing_main : MonoBehaviour
{
    public Animator animator;
    public Image BarFishing_Image;
    public FishingController fishingController; 
    MainPlayer mainPlayer;

    public event Action<int> OnEndFishing = delegate { };

    private void Start() {
        mainPlayer = GetComponentInParent<MainPlayer>();
        gameObject.SetActive(false);
    }

    void Update()
    {
        BarFishingdown_Image();
        if (Input.GetKeyDown(KeyCode.Space)) { BarFishingSpacebarUp_Image();  }
        OnFishing();
    }

    void BarFishingdown_Image()
    {
        BarFishing_Image.fillAmount -= Time.deltaTime * 0.05f;
    }

    void BarFishingSpacebarUp_Image()
    {
        BarFishing_Image.fillAmount += 0.05f;
    }

    void OnFishing()
    {
        if (BarFishing_Image.fillAmount == 0)
        {
            animator.SetBool("isFishing", false);
            gameObject.SetActive(false);
        }
        else if (BarFishing_Image.fillAmount == 1)
        {
            animator.SetBool("isFishing", false);
            OnEndFishing(fishingController.fishingRod.gainFish);
            gameObject.SetActive(false);
        }
    }

    


}
