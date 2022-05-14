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
    // public float rotationSpeed = 10f;
    
    

    [Header("Camera")]
    public CinemachineVirtualCamera CinemachineCamera;
    public GameObject CinemachineCameraTarget;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    public bool LockCameraPosition = false;
    

    public event Action SetPlayerUI = delegate { };
    public event Action<Vector3,Transform> MoveRotate = delegate { };

    Rigidbody rigidbody;

    protected void Awake()
    {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Start() 
    {
        rigidbody = this.GetComponent<Rigidbody>();

        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
        _input = GetComponent<StarterAssetsInputs>();
        _playerInput = GetComponent<PlayerInput>();
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
    }

    private void FixedUpdate() 
    {
        if(IsOwner && IsLocalPlayer)
        {
            MoveForward();
        }
    }

    protected void LateUpdate()
    {
        CameraRotation();
    }

    void MoveForward()
    {
        float Vertical = Input.GetAxis("Vertical");
        float Horizontal = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(Horizontal,0,Vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            MoveRotate(direction,_mainCamera.transform);

            // Vector3 moveDir = Quaternion.Euler(0f,targetAngle,0f) * Vector3.forward;

            direction = Camera.main.transform.TransformDirection(direction);
            direction.y = 0.0f;

            // rigidbody.velocity = direction * speed * Time.deltaTime;
            transform.Translate(direction * speed * Time.deltaTime);
        }

        // rigidbody.MovePosition(rigidbody.position + translation);
    }

    // void MoveRotation()
    // {
    //     float rotation = Input.GetAxis("Horizontal");

    //     if(rotation != 0)
    //     {
    //         rotation *= rotationSpeed;
    //         Quaternion turn = Quaternion.Euler(0f,rotation,0f);
    //         rigidbody.MoveRotation(rigidbody.rotation * turn);
    //     }
    //     else
    //     {
    //         rigidbody.angularVelocity = Vector3.zero;
    //     }
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

    protected void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    protected static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }


}