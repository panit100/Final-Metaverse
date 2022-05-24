using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ChatManager : MonoBehaviour
{   
    
    private void Start() {
        FindObjectOfType<LoginManager>().SetChatAndOverLayUI += SettingChatAndOverlay;    
    }

    // Update is called once per frame
    void SettingChatAndOverlay()
    {
        MainPlayer[] player = FindObjectsOfType<MainPlayer>();

        foreach(MainPlayer n in player)
        {
            if(n.GetComponent<NetworkObject>().IsOwner)
            {
                n.ChatCanvas.SetActive(true);
                n.fishingRodUI.SetActive(false);
                n.coinUI.SetActive(true);
            }
        }
    }
}
