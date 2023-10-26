using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MOVE : MonoBehaviour
{
    public CharacterController characterController { get; private set; }
    [SerializeField] private CinemachineInputProvider cinemachineInput;
    public TimeManager timeManager;
    float speed = 10f;
    float jumpSpeed = 5f;
    private float ySpeed;
    public Transform cam;
    Vector3 moveDir;
    public float turnSmoothTime = 0.4f;
    float turnSmoothVelocity;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        timeManager = GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        var vInput = Input.GetAxis("Vertical");
        var hInput = Input.GetAxis("Horizontal");
        Vector3 movementDirection = new Vector3(hInput, 0f, vInput);
        //movementDirection.Normalize();
        //movementDirection.y = ySpeed;   
        if (characterController.isGrounded)
        {
            ySpeed = -0.5f;

            if (Input.GetKey(KeyCode.Space))
            {
                ySpeed = jumpSpeed;
            }
        }
        if (movementDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(movementDirection.x, movementDirection.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir.Normalize();
            moveDir.y = ySpeed;
            ySpeed += Physics.gravity.y * Time.deltaTime;
            characterController.Move(moveDir * Time.deltaTime * speed);
        }
        if (Input.GetMouseButtonUp(0))
        {
            UnLockCamera();

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Star")
        {
            Debug.Log("Hello: " + other.name);
            if (Input.GetMouseButton(0))
            {
                LockCamera();
                timeManager.DoSlowmotion();

            }
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
