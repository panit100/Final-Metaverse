using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor;

[System.Serializable]
public class ClientData
{
    public ulong clientId;
    public string name;
    public FishingRodScriptableObject fishingRod;
    public bool isFishing;
    public string chatText;
    public int fishCoin;
    public int goldCoin;

    public ClientData(ulong _clientId,string _name)
    {
        name = _name;
        clientId = _clientId;
    }
}

public class MainPlayer : NetworkBehaviour
{
    Rigidbody rigidbody;
    GameObject _mainCamera;
    StarterAssetsInputs _input;
    PlayerInput _playerInput;
    const float _threshold = 0.01f;
    float _cinemachineTargetYaw;
    

    public ClientData clientData;

    [Header("Camera")]
    public CinemachineVirtualCamera CinemachineCamera;
    public GameObject CinemachineCameraTarget;

    [Header("Chat")]
    public GameObject ChatCanvas;
    public InputField inputText;
    public bool isTyping = false;

    [Header("FishingText")]
    public GameObject fishingText;

    public event Action SetPlayerNameUI = delegate { };
    public event Action SetPlayerChatText = delegate { };
    public event Action<GameObject,Rigidbody> MovePosition = delegate { };
    public event Action Fishing = delegate { };
    public event Action ShowSpaceBar = delegate { };
    public event Action<ClientData,int> SetGoldCoin = delegate { };
    public event Action<ClientData,int> SetFishCoin = delegate { };
    public event Action<ClientData> PlayGacha = delegate { };
    public event Action SetCoinUI = delegate { };
    public event Action SetFishingRodUI = delegate { }; 
    
    protected void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start() 
    {
        if(IsOwner && IsLocalPlayer)
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
         
            _input = GetComponent<StarterAssetsInputs>();
            // _input.SetCursorState(cursorState);

            _playerInput = GetComponent<PlayerInput>();
            _playerInput.enabled = true;

            PlayGacha += FindObjectOfType<FishingRodGacha>().GachaRandom;

            //Test.enabled = false;
            
        }

        rigidbody = GetComponent<Rigidbody>();
        
        GetComponentInChildren<GameFishing_main>().OnEndFishing += OnEndFishingServerRpc;
        GetComponentInChildren<FishingController>().HandleFishing += HandleFishingServerRpc;

#if UNITY_EDITOR
        clientData.fishingRod = (FishingRodScriptableObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/RookieRod.asset", typeof(FishingRodScriptableObject));
#else
        clientData.fishingRod = Resources.Load("RookieRod") as FishingRodScriptableObject;
#endif
    }

    public void Initialization(string name)
    {
        ChangeNamgeServerRpc(new DataCollect(name));
    }

    private void Update() 
    {
        if(IsOwner && IsLocalPlayer)
        {
            HandleUI();
        }
        
        HandleSetName();

        if(inputText.isFocused)
        {
            isTyping = true;
        }else
        {
            isTyping = false;
        }
    }

    private void FixedUpdate() 
    {
        if(isTyping) return;

        if(!clientData.isFishing){
            HandleMove();
        }

        if(IsOwner && IsLocalPlayer)
        {
            ShowSpaceBar();

            HandleFishing();
            HandlePlayGacha();
        }
    }

    //Move
    void HandleMove()
    {
        if(IsOwner && IsLocalPlayer)
        {
            MovePosition(_mainCamera,rigidbody);
        }
    }

    //Name
    void HandleSetName()
    {
        if(clientData.name != "")
        {
            SetPlayerNameUI();
        }   
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void ChangeNamgeServerRpc(DataCollect data)
    {
        ChangeNamgeClientRpc(data);
    }

    [ClientRpc]
    void ChangeNamgeClientRpc(DataCollect data)
    {
        clientData.name = data.playerName;
        SetPlayerNameUI();
    }

    //Fishing
    public void HandleFishing()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(!clientData.isFishing)
            {
                Fishing();
            }
        }
    }
    
    [ServerRpc]
    public void HandleFishingServerRpc()
    {
        HandleFishingClientRpc();
    }

    [ClientRpc]
    public void HandleFishingClientRpc()
    {
        clientData.isFishing = true;
        fishingText.SetActive(true);
    }

    [ServerRpc]
    public void OnEndFishingServerRpc(int coin)
    {
        OnEndFishingClientRpc(coin);
    }

    [ClientRpc]
    public void OnEndFishingClientRpc(int coin)
    {
        clientData.isFishing = false;
        fishingText.SetActive(false);
        SetFishCoin(clientData,coin);
    }

    //Chating
    public void OnChatTextChange()
    {
        clientData.chatText = inputText.text;
        inputText.text = "";
        HandleChatingServerRpc(new DataChatCollect(clientData.chatText));
    }

    [ServerRpc(RequireOwnership = false)]
    public void HandleChatingServerRpc(DataChatCollect data)
    {
        HandleChatingClientRpc(data);
    }

    [ClientRpc]
    public void HandleChatingClientRpc(DataChatCollect data)
    {
        clientData.chatText = data.chatText;
        SetPlayerChatText();
    }

    //Gacha
    void HandlePlayGacha()
    {
        if(Input.GetKeyDown(KeyCode.Space) && (clientData.fishCoin >= 10 || clientData.goldCoin >= 10))
        {
            OnGachaShop();
        }
    }
    void OnGachaShop()
    {
        Collider[] hit = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider n in hit)
        {
            if(n.CompareTag("Gacha"))
            { 
                if(clientData.fishCoin >= 10)
                {
                   clientData.fishCoin -= 10;
                }else if(clientData.goldCoin >= 10)
                {
                    clientData.goldCoin -= 10;
                }

                PlayGacha(clientData);
                Debug.Log("PlayGacha");
            }
        }
        return;
    }

    void HandleUI()
    {
        SetCoinUI();
        SetFishingRodUI();
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}