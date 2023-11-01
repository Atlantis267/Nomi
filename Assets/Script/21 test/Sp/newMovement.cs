using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class newMovement : MonoBehaviour
{
    //input
    private PlayerInput1 playerinput;
    private InputAction move;

    [Header("References")]
    private Rigidbody rb;
    Animator animator;
    [SerializeField]private Camera playerCamera;
    public float playerHeight = 2.0f;
    private Vector2 moveAxis;
    //Acceleration and deceleration
    public float SpeedChangeRate = 10.0f;
    int VelocityHash;



    [Header("Movement")]
    [SerializeField]private float moveSpeed;
    [SerializeField] private float sprintSpeed;
    Vector3 moveDirection;
    private float rotatespeed = 7f;
    private float maxspeed = 10f;

    //public float walkSpeed;
    //public float sprintSpeed;

    //public float dashSpeed;
    //public float dashSpeedChangeFactor;

    //public float maxYSpeed;

    public float groundDrag;

    [Header("Jumping")]
    //public float jumpForce;
    //public float jumpCooldown;
    //public float airMultiplier;
    bool readyToJump;
    [Header("Bools")]
    bool isSprintPress;

    //animation 
    private float _animationBlend;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerinput = new PlayerInput1();
        VelocityHash = Animator.StringToHash("Blend");
    }

    private void DoSpirnt(InputAction.CallbackContext context)
    {
        isSprintPress = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        move = playerinput.CharacterControl.Movement;
        playerinput.CharacterControl.Sprinting.started += DoSpirnt;
        playerinput.CharacterControl.Sprinting.canceled += DoSpirnt;
        playerinput.CharacterControl.Enable();
    }
    private void OnDisable()
    {
        playerinput.CharacterControl.Disable();
    }
    /*public void OnMove(InputValue value)
    {
        moveAxis.x = value.Get<Vector2>().x;
        moveAxis.y = value.Get<Vector2>().y;
    }*/

    private void FixedUpdate()
    {
        MovePlayer();
        Lookat();
    }

    private void MovePlayer()
    {
        moveDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * moveSpeed;
        moveDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * moveSpeed;

        rb.AddForce(moveDirection, ForceMode.Impulse);
        moveDirection = Vector3.zero;

        if (IsGrounded())
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        Vector3 horizontalVel = rb.velocity;
        horizontalVel.y = 0.0f;
        if (horizontalVel.sqrMagnitude > maxspeed * maxspeed)
            rb.velocity = horizontalVel.normalized * maxspeed + Vector3.up * rb.velocity.y;

        /*float targetSpeed = isSprintPress ? sprintSpeed : moveSpeed;
        //if(moveAxis.x < 0 || moveAxis.y < 0) targetSpeed = 0.0f;
        moveDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera);
        moveDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera);

        rb.AddForce(moveDirection * targetSpeed, ForceMode.Force);
        moveDirection = Vector3.zero;

        if (IsGrounded())
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        Vector3 horizontalVel = rb.velocity;
        horizontalVel.y = 0.0f;
        float speedOffset = 0.1f;
        float inputMagnitude = new Vector2(moveAxis.x, moveAxis.y).magnitude;
        float _speed;
        /*if (horizontalVel.magnitude > targetSpeed + speedOffset || horizontalVel.magnitude < targetSpeed - speedOffset)
        {
            _speed = Mathf.Lerp(horizontalVel.magnitude, targetSpeed * inputMagnitude,
                   Time.deltaTime * SpeedChangeRate);

            // round speed to 3 decimal places
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        if(horizontalVel.magnitude > targetSpeed)
        {
            Vector3 limitedVel = horizontalVel.normalized * targetSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
       
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        animator.SetFloat(VelocityHash, _animationBlend);*/
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }
    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }
    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, playerHeight * 0.5f + 0.2f))
            return true;
        else
            return false;               
    }
    private void Lookat()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            transform.forward = Vector3.Slerp(transform.forward, direction.normalized, Time.deltaTime * rotatespeed);
        else
            rb.angularVelocity = Vector3.zero;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovementAnimation();
    }

    private void HandleMovementAnimation()
    {
        animator.SetFloat("Blend", rb.velocity.magnitude/maxspeed);
    }

}
