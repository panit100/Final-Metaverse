using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFishRodUI : MonoBehaviour
{
    public GameObject fishingRod;
    public Image rodImg;
    public Text rodName;
    ClientData client;
    // Start is called before the first frame update
    void Start()
    {
        client = GetComponentInParent<MainPlayer>().clientData;
        GetComponentInParent<MainPlayer>().SetFishingRodUI += ShowFishingRodUI;
    }

    // Update is called once per frame
    void Update()
    {
        SetRodImg();
        SetRodName();
    }

    public void ShowFishingRodUI()
    {
        fishingRod.SetActive(true);
    }

    public void SetRodImg()
    {
        rodImg.sprite = client.fishingRod.image;
    }

    public void SetRodName()
    {
        rodName.text = client.fishingRod.name;
    }
}
