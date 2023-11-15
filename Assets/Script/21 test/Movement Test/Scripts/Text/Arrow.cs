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

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Dashing")]
    public float dashForce;
    public float dashUpwardForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    private Vector3 delayedForceToApply;
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

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
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
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        PlayerMovementDashing pm = other.GetComponent<PlayerMovementDashing>();

        if (other.tag == "Player")
        {
            if (slow)
            {
                should = true;
                other.transform.rotation = transform.rotation;
                Debug.Log("1");
                DoSlowmotion();
                Arrows.SetActive(true);
            }else if (!slow && should)
            {
               
                if (dashCdTimer > 0) return;
                else dashCdTimer = dashCd;

                pm.dashing = true;
                pm.maxYSpeed = maxDashYSpeed;

                Vector3 forceToApply = transform.forward * dashForce + transform.up * dashUpwardForce;

                if (disableGravity)
                    otherRigidbody.useGravity = false;
                delayedForceToApply = forceToApply;

                if (resetVel)
                    otherRigidbody.velocity = Vector3.zero;

                otherRigidbody.AddForce(delayedForceToApply, ForceMode.Impulse);
                Debug.Log("3");
                Time.timeScale = starttime;
                Time.fixedDeltaTime = startfix;
                Arrows.SetActive(false);
                should = false;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        PlayerMovementDashing pm = other.GetComponent<PlayerMovementDashing>();

        pm.dashing = false;
        pm.maxYSpeed = 0;

        if (disableGravity)
            otherRigidbody.useGravity = true;

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
