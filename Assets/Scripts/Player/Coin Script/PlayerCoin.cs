using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoin : MonoBehaviour
{
    private void Start() {
        GetComponentInParent<MainPlayer>().SetGoldCoin += SetPlayerGoldCoin;
        GetComponentInParent<MainPlayer>().SetFishCoin += SetPlayerFishingCoin;
    }

    public void SetPlayerGoldCoin(ClientData player,int coin)
    {
        player.goldCoin += coin;
    }

    public void SetPlayerFishingCoin(ClientData player,int coin)
    {
        player.fishCoin += coin;
    }
}
