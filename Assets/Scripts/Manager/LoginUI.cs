using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginUI : MonoBehaviour
{
    public GameObject connectUI;
    public GameObject disconnectUI;

    private void Start() {
        HandleDisconnectAndStartServer();
        FindObjectOfType<LoginManager>().connectedEvent += HandleConnectServer;
        FindObjectOfType<LoginManager>().disconnectedEvent += HandleDisconnectAndStartServer;
    }

    void HandleDisconnectAndStartServer()
    {
        connectUI.SetActive(true);
        disconnectUI.SetActive(false);
    }

    void HandleConnectServer()
    {
        connectUI.SetActive(false);
        disconnectUI.SetActive(true);
    }
}
