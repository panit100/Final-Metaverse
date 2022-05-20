using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFishRodUI : MonoBehaviour
{
    public Image rodImg;
    public Text rodName;
    ClientData client;
    // Start is called before the first frame update
    void Start()
    {
        client = GetComponentInParent<MainPlayer>().clientData;
    }

    // Update is called once per frame
    void Update()
    {
        SetRodImg();
        SetRodName();
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
