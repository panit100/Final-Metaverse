using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CameraManager : MonoBehaviour
{   
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
