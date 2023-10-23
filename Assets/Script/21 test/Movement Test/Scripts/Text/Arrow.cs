using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Arrow : MonoBehaviour
{
    public float speed;
    [SerializeField] private CinemachineInputProvider cinemachineInput;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            LockCamera();
            float mouseX = Input.GetAxisRaw("Mouse X") * Mathf.Deg2Rad * speed;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Mathf.Deg2Rad * speed;

            transform.RotateAround(Vector3.up, -mouseX);
        }
        if (Input.GetMouseButtonUp(0))
        {
            UnLockCamera();
        }      
    }
    void LockCamera()
    {
        if (cinemachineInput != null)
            cinemachineInput.enabled = false;

    }
    void UnLockCamera()
    {
        if (cinemachineInput != null)
            cinemachineInput.enabled = true;
    }
}
