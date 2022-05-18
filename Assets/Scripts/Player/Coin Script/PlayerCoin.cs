using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoin : MonoBehaviour
{
    private void Start() {
        GetComponent<MainPlayer>().SetCoin += SetPlayerCoin;
    }

    public void SetPlayerCoin(ClientData player,int coin)
    {
        player.GoldCoin += coin;
    }
}
