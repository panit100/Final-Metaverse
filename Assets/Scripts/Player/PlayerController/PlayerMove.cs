using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMove : MonoBehaviour
{
    bool PlayerFishingBool = false;

    public float speed = 5;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public event Action<Vector3,Transform> MoveRotate = delegate { };

    private void Start() {
        GetComponent<MainPlayer>().MovePosition += MovePosition;
       //GetComponent<FishingController>().PlayerFishing += PlayerFishing;
    }

    void MovePosition(GameObject camera,Rigidbody rigidbody)
    {

        if (PlayerFishingBool == false) 
        {
            float Vertical = Input.GetAxis("Vertical");
            float Horizontal = Input.GetAxis("Horizontal");

            Vector3 direction = new Vector3(Horizontal, 0, Vertical).normalized;

            if (direction.magnitude >= 0.1f)
            {
                MoveRotate(direction, camera.transform);
                direction = Camera.main.transform.TransformDirection(direction);
                direction.y = 0.0f;

                transform.Translate(direction * speed * Time.deltaTime);
            }
            else
            {
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }

            transform.rotation = Quaternion.identity;

        }
    }

    public void PlayerFishing()
    {
        PlayerFishingBool = !PlayerFishingBool;
    }


}
