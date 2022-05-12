using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class GUIMultiplayManager : NetworkBehaviour
{

    private void OnGUI() {
        GUILayout.BeginArea(new Rect(10,10,300,300));
        if(!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButton();
        }else{
            StatusLabels();

            SubmitNewPosition();
        }

        GUILayout.EndArea();
    }

    private void StartButton()
    {
        if(GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if(GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if(GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }

    private void StatusLabels()
    {
        var mode = NetworkManager.Singleton.IsHost ?
                    "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";
        GUILayout.Label("Transport: " +
                    NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }

    private void SubmitNewPosition()
    {
        // if(GUILayout.Button(NetworkManager.Singleton.IsServer ? "Move" : "Request Position Change"))
        // {
        //     var playerObject = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();
        //     var player = playerObject.GetComponent<MainPlayer>();
        //     player.Move();
        // }
    }


    
}
