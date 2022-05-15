using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraManager : MonoBehaviour
{   
    // MainPlayer localPlayer;
    // private void Start() {
    //     // FindObjectOfType<MainPlayer>().SetCamera += SettingCamera;    
    //     localPlayer = GetComponentInParent<MainPlayer>();
    //     SettingCamera();
    // }

    // // Update is called once per frame
    // void SettingCamera()
    // {
    //     if(localPlayer.GetComponent<NetworkObject>().IsLocalPlayer)
    //     {
    //         localPlayer.CinemachineCamera.gameObject.SetActive(true);
    //     }
    // }
    private void Start() {
        FindObjectOfType<LoginManager>().SetCamera += SettingCamera;    
    }

    // Update is called once per frame
    void SettingCamera()
    {
        MainPlayer[] player = FindObjectsOfType<MainPlayer>();

        foreach(MainPlayer n in player)
        {
            if(n.GetComponent<NetworkObject>().IsOwner)
            {
                n.CinemachineCamera.gameObject.SetActive(true);
            }
        }
    }

    
}
