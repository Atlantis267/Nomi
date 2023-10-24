using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Pull : MonoBehaviour
{
    float xroat, yroat = 0f;
    public Rigidbody ball;
    public float rotatespeed = 5f;
    public LineRenderer line;
    public TimeManager timeManager;
    public float shootpower = 30f;
    public float maxpower = 50f;
    [SerializeField] private CinemachineInputProvider cinemachineInput;
    private void Start()
    {
        timeManager = GetComponent<TimeManager>();
    }
    void Update()
    {
        transform.position = ball.position;
        Vector3 inputPos = new Vector3(xroat, yroat, xroat);
        if (Input.GetMouseButton(0))
        {
            LockCamera();
            timeManager.DoSlowmotion();
            xroat += Input.GetAxis("Mouse X") * rotatespeed;
            yroat += Input.GetAxis("Mouse Y") * rotatespeed;         
            transform.rotation = Quaternion.Euler(yroat, xroat, 0f);
            line.gameObject.SetActive(true);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + transform.forward * 4f);
            if (yroat < -35f)
            {
                yroat = -35f;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            ball.velocity = transform.forward * shootpower;
            line.gameObject.SetActive(false);
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
