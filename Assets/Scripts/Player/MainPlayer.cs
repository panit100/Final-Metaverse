using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

[System.Serializable]
public class ClientData
{
    public ulong clientId;
    public string name;
    public bool isFishing;

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


    public event Action SetPlayerUI = delegate { };
    public event Action<GameObject,Rigidbody> MovePosition = delegate { };
    public event Action Fishing = delegate { };
    public event Action ShowSpacebar = delegate { };


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
        }

        rigidbody = GetComponent<Rigidbody>();
    }

    public void Initialization(string name)
    {
        ChangeNamgeServerRpc(new DataCollect(name));
    }

    private void Update() 
    {
        if(clientData.name != "")
        {
            SetPlayerUI();
        }    




        if(IsOwner && IsLocalPlayer)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(!clientData.isFishing)
                {
                    HandleFishingServerRpc();
                }
            }

        }

    }

    private void FixedUpdate() 
    {
        if(IsOwner && IsLocalPlayer)
        {
            MovePosition(_mainCamera,rigidbody);
        }
    }

    // void MoveForward()
    // {
    //     float Vertical = Input.GetAxis("Vertical");
    //     float Horizontal = Input.GetAxis("Horizontal");

    //     // transform.rotation = Quaternion.identity;

    //     // rigidbody.velocity = Vector3.zero;
    //     // rigidbody.angularVelocity = Vector3.zero;

    //     Vector3 direction = new Vector3(Horizontal,0,Vertical).normalized;

    //     if(direction.magnitude >= 0.1f)
    //     {
    //         MoveRotate(direction,_mainCamera.transform);
    //         direction = Camera.main.transform.TransformDirection(direction);
    //         direction.y = 0.0f;

    //         transform.Translate(direction * speed * Time.deltaTime);
    //     }
    //     else
    //     {
    //         rigidbody.velocity = Vector3.zero;
    //         rigidbody.angularVelocity = Vector3.zero;
    //     }

    //     transform.rotation = Quaternion.identity;
    // }

    [ServerRpc(RequireOwnership = false)]
    public void ChangeNamgeServerRpc(DataCollect data)
    {
        ChangeNamgeClientRpc(data);
    }

    [ClientRpc]
    void ChangeNamgeClientRpc(DataCollect data)
    {
        clientData.name = data.playerName;
        SetPlayerUI();
    }


    [ServerRpc]
    public void HandleFishingServerRpc()
    {
        HandleFishingClientRpc();
    }

    [ClientRpc]
    public void HandleFishingClientRpc()
    {
        Fishing();
        clientData.isFishing = true;
    }
}