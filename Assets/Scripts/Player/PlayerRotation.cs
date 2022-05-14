using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    private void Start() {
        GetComponentInParent<MainPlayer>().MoveRotate += MoveRotation;
    }

    void MoveRotation(Vector3 direction,Transform _mainCamera)
    {
        float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f,angle,0f);
    }
}
