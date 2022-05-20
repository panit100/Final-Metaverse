using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;
using System.Text;

public class PlayerNameUI : NetworkBehaviour
{
    public Text nameText;
    public string clientName;

    private void Start() 
    {
        GetComponentInParent<MainPlayer>().SetPlayerNameUI += SetPlayerName;
    }

    public void SetPlayerName()
    {
        clientName = GetComponentInParent<MainPlayer>().clientData.name;
        nameText.text = clientName;
    }
}