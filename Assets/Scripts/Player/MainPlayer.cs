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
    public string name;

    public ClientData(string _name)
    {
        name = _name;
    }
}

public class MainPlayer : NetworkBehaviour
{
    Rigidbody rigidbody;
    public GameObject _mainCamera;
    StarterAssetsInputs _input;
    PlayerInput _playerInput;
    const float _threshold = 0.01f;
    float _cinemachineTargetYaw;
    float _cinemachineTargetPitch;
    bool IsCurrentDeviceMouse
    {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
    }
    
    public ClientData clientData;
    public float speed = 5f;

    [Header("Camera")]
    public CinemachineVirtualCamera CinemachineCamera;
    public GameObject CinemachineCameraTarget;
    // public float TopClamp = 70.0f;
    // public float BottomClamp = -30.0f;
    // public float CameraAngleOverride = 0.0f;
    // public bool LockCameraPosition = false;
    // public bool cursorState = false;
    

    public event Action SetPlayerUI = delegate { };
    // public event Action SetCamera = delegate { };
    public event Action<Vector3,Transform> MoveRotate = delegate { };

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
            _input.SetCursorState(true);

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
            // SetCursorState();
        }
    }

    private void FixedUpdate() 
    {
        if(IsOwner && IsLocalPlayer)
        {
            MoveForward();
            // CameraRotation();
        }
    }

    void MoveForward()
    {
        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");

        transform.rotation = Quaternion.identity;

        // rigidbody.velocity = Vector3.zero;
        // rigidbody.angularVelocity = Vector3.zero;

        Vector3 direction = new Vector3(Horizontal,0,Vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            MoveRotate(direction,_mainCamera.transform);

            direction = Camera.main.transform.TransformDirection(direction);
            direction.y = 0.0f;

            transform.Translate(direction * speed * Time.deltaTime);
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
        SetPlayerUI();
    }

    // protected void CameraRotation()
    // {
    //     // if there is an input and camera position is not fixed
    //     if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
    //     {
    //         //Don't multiply mouse input by Time.deltaTime;
    //         float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

    //         _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
    //         _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
    //     }

    //     // clamp our rotations so our values are limited 360 degrees
    //     _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
    //     _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

    //     // Cinemachine will follow this target
    //     CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
    //         _cinemachineTargetYaw, 0.0f);
    // }

    // protected static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    // {
    //     if (lfAngle < -360f) lfAngle += 360f;
    //     if (lfAngle > 360f) lfAngle -= 360f;
    //     return Mathf.Clamp(lfAngle, lfMin, lfMax);
    // }

    // public void SetCursorState(){
    //     if(Input.GetKeyDown(KeyCode.Escape)){
    //         _input.SetCursorState(!cursorState);
    //     }
    // }


}