using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using System;

public class Arrow : MonoBehaviour
{
    public float speed;
    float mouseX /*mouseY*/ = 0f;
    private float starttime;
    private float startfix;
    public float slowdownFactor = 0.05f;
    [SerializeField] private CinemachineInputProvider cinemachineInput;
    [SerializeField] private GameObject Arrows;
    private TestInput input = null;
    public float shootpower = 30f;
    public bool slow;
    public bool should;


    private void Awake()
    {
        input = new TestInput();
        starttime = Time.timeScale;
        startfix = Time.fixedDeltaTime;
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            LockCamera();
            mouseX += Input.GetAxisRaw("Mouse X") * speed;
             //mouseY = Input.GetAxisRaw("Mouse Y") * Mathf.Deg2Rad * speed;

            transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
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
    private void OnTriggerStay(Collider other)
    {      
        if (other.tag == "Player")
        {
            if (slow)
            {
                should = true;
                other.transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
                Debug.Log("1");
                DoSlowmotion();
                Arrows.SetActive(true);
            }else if (!slow && should)
            {
                Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
                otherRigidbody.AddForce(transform.forward * shootpower, ForceMode.Impulse);
                Debug.Log("3");
                Time.timeScale = starttime;
                Time.fixedDeltaTime = startfix;
                Arrows.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("2");
        if (other.tag == "Player")
        {
            Time.timeScale = starttime;
            Time.fixedDeltaTime = startfix;
            should = false;
        }
    }
    void DoSlowmotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
    }
    private void OnEnable()
    {
        input.Enable();
        input.Locktest.Slowmotion.started += OnSlowmotion;
        input.Locktest.Slowmotion.canceled += OnSlowmotionCanceled;
    }

    private void OnSlowmotionCanceled(InputAction.CallbackContext obj)
    {
        slow = false;
    }

    private void OnSlowmotion(InputAction.CallbackContext obj)
    {
        slow = true;
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
