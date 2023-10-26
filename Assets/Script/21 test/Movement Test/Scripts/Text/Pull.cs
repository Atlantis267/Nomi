using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Pull : MonoBehaviour
{
    public float slowdownFactor = 0.05f;
    private float starttime;
    private float startfix;
    float xroat, yroat = 0f;
    public Rigidbody ball;
    public float rotatespeed = 5f;
    public LineRenderer line;
    public float shootpower = 30f;
    public float maxpower = 50f;
    [SerializeField] private CinemachineInputProvider cinemachineInput;

    private void Awake()
    {
        starttime = Time.timeScale;
        startfix = Time.fixedDeltaTime;
    }
    void Update()
    {
        Time.timeScale = starttime;
        Time.fixedDeltaTime = startfix;
        transform.position = ball.position;
        Vector3 inputPos = new Vector3(xroat, yroat, xroat);
        if (Input.GetMouseButton(0))
        {
            LockCamera();
            DoSlowmotion();          
            xroat += Input.GetAxis("Mouse X") * rotatespeed;
            //yroat += Input.GetAxis("Mouse Y") * rotatespeed;         
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
    void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
}
