using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;
using System.Text;

public class PlayerUI : NetworkBehaviour
{
    public Text nameText;
    // public NetworkVariable<NetworkString> clinetname = new NetworkVariable<NetworkString>();
    public string clientName;

    private void Start() 
    {
        // clientName.Value = Encoding.ASCII.GetBytes(GetComponentInParent<MainPlayer>().clientData.name);
        // clinetname.Value = "String";
        // SetPlayerName(clinetname.ToString());

        GetComponentInParent<MainPlayer>().SetPlayerUI += SetPlayerName;
    }

    public void SetPlayerName()
    {
        clientName = GetComponentInParent<MainPlayer>().clientData.name;
        nameText.text = clientName;
    }
}