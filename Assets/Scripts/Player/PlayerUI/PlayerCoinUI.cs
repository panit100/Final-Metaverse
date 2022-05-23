using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCoinUI : MonoBehaviour
{
    public GameObject coin;
    public Text FishCoinText;
    public Text GoldCoinText;

    ClientData client;

    private void Start() 
    {
        client = GetComponentInParent<MainPlayer>().clientData;
        GetComponentInParent<MainPlayer>().SetCoinUI += ShowCoinUI;
    }

    private void Update() 
    {
        SetFishCoinText();
        SetGoldCoinText();
    }

    public void ShowCoinUI()
    {
        coin.SetActive(true);
    }

    public void SetFishCoinText()
    {
        FishCoinText.text = "" + client.fishCoin;
    }

    public void SetGoldCoinText()
    {
        // chat = _chat;
        GoldCoinText.text = "" + client.goldCoin;
    }
}
