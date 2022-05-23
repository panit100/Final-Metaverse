using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;
using System.Text;


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

    [Header("Transport")]
    public string joinCode;

    [Header("JoinCode")]
    public Text JoinCodeText;

    public event Action SetCamera = delegate { };
    public event Action SetChatUI = delegate { };
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
        HandleSetChatUIServerRpc();
        
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

    [ServerRpc]
    void HandleSetChatUIServerRpc()
    {
        HandleSetChatUIClientRpc();
    }

    [ClientRpc]
    void HandleSetChatUIClientRpc()
    {
        SetChatUI();
    }    

    public void OnIpAddressChanged(string address)
    {
        this.joinCode = address;
    }

    public async void Host() 
    {
        if(RelayManager.Instance.IsRelayEnabled)
        {
            await RelayManager.Instance.SetupRelay();
        }

        NetworkManager.Singleton.NetworkConfig.ConnectionData =
            System.Text.Encoding.ASCII.GetBytes(playerNameInputField.text);
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost();
    }

    public async void Client() 
    {
        if(RelayManager.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCode))
        {
            await RelayManager.Instance.JoinRelay(joinCode);
        }

        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(playerNameInputField.text);
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
        bool approve1 = GetPlayerName(clientId,Approve);

        Vector3 spawnPosition = SetPlayerPosition();
        Quaternion spawnRotation = Quaternion.identity;

        bool createPlayerObject = true;
        callback(createPlayerObject, null, approve1, spawnPosition, null);
    }

    bool GetPlayerName(ulong clientId,string clientName)
    {
        foreach(ClientData clientData in clientDatas){
            if(clientName == clientData.name){
                return false;
            }
        }

        clientDatas.Add(new ClientData(clientId,clientName));

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
                NetworkManager.Singleton.ConnectedClients[(ulong)i].PlayerObject.GetComponent<MainPlayer>().Initialization(clientDatas[i].name);
            }
        }
    }

    public void HandleJoinCodeUI()
    {
        JoinCodeText.text = "Join Code : " + joinCode;
    }
}
