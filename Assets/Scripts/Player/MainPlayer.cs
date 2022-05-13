using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;
using Cinemachine;
using Unity.Networking.Transport;

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
    public ClientData clientData;
    public float speed = 5f;
    public float rotationSpeed = 10f;
    public CinemachineVirtualCamera Vcam;

    public event Action SetPlayerUI = delegate { };

    Rigidbody rigidbody;

    private void Start() 
    {
        rigidbody = this.GetComponent<Rigidbody>();
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
            MoveRotation();
        }
    }

    void MoveForward()
    {
        float translation = Input.GetAxis("Vertical") * speed;

        translation  *= Time.deltaTime;

        rigidbody.MovePosition(rigidbody.position + this.transform.forward * translation);
    }

    void MoveRotation()
    {
        float rotation = Input.GetAxis("Horizontal");

        if(rotation != 0)
        {
            rotation *= rotationSpeed;
            Quaternion turn = Quaternion.Euler(0f,rotation,0f);
            rigidbody.MoveRotation(rigidbody.rotation * turn);
        }
        else
        {
            rigidbody.angularVelocity = Vector3.zero;
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


}