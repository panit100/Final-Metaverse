using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;
using System.Text;
using Unity.Networking.Transport;


public class LoginManager : MonoBehaviour
{
    [Header("Button")]
    public Button hostButton;
    public Button clientButton;
    public Button LogoutButton;

    [Header("Input")]
    public Text playerNameInputField;
    public Text passwordInputfield;

    [Header("Client")]
    public List<ClientData> clientDatas = new List<ClientData>();
    public string password;

    public event Action SetCamera = delegate { };
    public event Action connectedEvent = delegate { };
    public event Action disconnectedEvent = delegate { };

    private void Start() {
        AddEventToButton();

        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    }

    void AddEventToButton(){
        hostButton.GetComponent<Button>().onClick.AddListener(Host);
        clientButton.GetComponent<Button>().onClick.AddListener(Client);
        LogoutButton.GetComponent<Button>().onClick.AddListener(Logout);
    }

    private void OnDestroy() {
        if(NetworkManager.Singleton == null) { return; }
        
        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        if(clientId == NetworkManager.Singleton.LocalClientId){
            disconnectedEvent();
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        HandleSetCameraServerRpc();
        
        if(clientId == NetworkManager.Singleton.LocalClientId){
            connectedEvent();
        }

        SetClientData();
    }

    private void HandleServerStarted()
    {

    }

    [ServerRpc]
    void HandleSetCameraServerRpc()
    {
        HandleSetCameraClientRpc();
    }

    [ClientRpc]
    void HandleSetCameraClientRpc()
    {
        SetCamera();
    }

    public void Host() 
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData =
           System.Text.Encoding.ASCII.GetBytes(playerNameInputField.text + "_" + passwordInputfield.text);
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }

    public void Client() 
    {
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(playerNameInputField.text + "_" + passwordInputfield.text);
        NetworkManager.Singleton.StartClient();
    }

    public void Logout() 
    {
        if(NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.Shutdown();
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
        else if(NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.Shutdown();
        }

        disconnectedEvent();
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        string Approve = Encoding.ASCII.GetString(connectionData);
        string[] Room = Approve.Split("_");
        bool approve1 = GetPlayerName(Room[0]);
        bool approve2 = ApprovePassword(Room[1]);
        
        // bool approveConnection = playerName != playerNameInputField.text;

        Vector3 spawnPosition = SetPlayerPosition();
        Quaternion spawnRotation = Quaternion.identity;

        // print("Count : " + NetworkManager.Singleton.ConnectedClients.Count);

        bool createPlayerObject = true;
        callback(createPlayerObject, null, approve1 && approve2, spawnPosition, null);
    }

    bool GetPlayerName(string clientName)
    {
        foreach(ClientData clientData in clientDatas){
            if(clientName == clientData.name){
                return false;
            }
        }

        clientDatas.Add(new ClientData(clientName));

        return true;
    }

    bool ApprovePassword(string _password)
    {
        if(password == ""){
            password = _password;
        }

        if(_password == password){
            return true;
        }

        return false;
    }

    Vector3 SetPlayerPosition()
    {
        if(NetworkManager.Singleton.ConnectedClients.Count == 0)
        {
            return new Vector3(2f,1f,0f);
        }
        else
        {
            switch (NetworkManager.Singleton.ConnectedClients.Count % 4)
            {
                case 0:
                    return new Vector3(1f,1f,1f);
                case 1: 
                    return new Vector3(0f,1f,0f);
                case 2: 
                    return new Vector3(-2f,1f,0f);
                case 3: 
                    return new Vector3(2f,1f,2f);
            }
        }

        return Vector3.zero;
    }

    void SetClientData()
    {
        if(NetworkManager.Singleton.IsServer)
        {
            for(int i = 0; i < NetworkManager.Singleton.ConnectedClients.Count; i++)
            {
                // Debug.Log(NetworkManager.Singleton.ConnectedClients[(ulong)i].ClientId);
                NetworkManager.Singleton.ConnectedClients[(ulong)i].PlayerObject.GetComponent<MainPlayer>().Initialization(clientDatas[i].name);
            }
        }
    }
}
