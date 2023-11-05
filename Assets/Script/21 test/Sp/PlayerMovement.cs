using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Transform playerTransform;
    PlayerInput1 _input;
    //private InputAction move;
    private CharacterController ch;
    private CapsuleCollider collider;
    public LayerMask layerMask;
    Animator animator;
    public Rigidbody rb;
    PlayerSensor playerSensor;
    [SerializeField] private Camera playerCamera;
    //State
    #region 
    public enum PlayerPosture
    {
        Stand,
        Falling,
        Jumping,
        Landing,
        Climbing
    };
    [HideInInspector]
    public PlayerPosture playerPosture = PlayerPosture.Stand;
    public enum LocomotionState
    {
        Idle,
        Move
    };
    [HideInInspector]
    public LocomotionState locomotionState = LocomotionState.Idle;

    float standThreshold = 1f;
    float jumpThreshold = 2.1f;
    float landingThreshold = 1f;
    #endregion
    //Hash
    #region
    int playerstateHash;
    int midairHash;
    int moveSpeedHash;
    int moveboolHash;
    int jumpboolHash;

    int verticalVelHash;
    int feetTweenHash;
    #endregion
    //Velocity Cache Pool Definition Author
    #region 
    static readonly int CACHE_SIZE = 3;
    Vector3[] velCache = new Vector3[CACHE_SIZE];
    int currentChacheIndex = 0;
    Vector3 averageVel = Vector3.zero;
    #endregion 
    //Input
    private Vector2 currentMovemevtInput;
    Vector3 currentMovemevt;
    Vector3 currentMovemevtWorldSpace = Vector3.zero;
    bool isSprintPressed;
    bool isMovementPressed;
    bool isJumpPressed;

    bool couldFall;
    bool isLanding;

    bool isClimbReady;

    //CheckGround
    [SerializeField]
    bool isGrounded = true;
    float groundCheckOffset = 0.5f;

    [SerializeField]
    private float maximumSpeed = 1f;
    private float sprintSpeed = 1.5f;
    public float groundDrag = 6f;
    public float playerHeight = 2.0f;
    public float RotationSmoothTime = 0.12f;
    //private float rotatespeed = 10f;
    //private float _speed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _yspeed;
    public float jumpForce = 2.0f;
    private float gravity = -9.81f;
    private float groundedGravity = -.9f;
    float maxJumpHeight = 1.0f;
    private float initialJumpVelocity;
    public float jumpspeed;
    float fallMultiplier = 2f;
    float feetTween;
    float jumpCD = 0.25f;
    float fallHeight = 20f;
    int defaultClimbParameter = 0;
    int vaultParameter = 1;
    int lowClimbParameter = 2;
    int highClimbParameter = 3;
    int currentClimbparameter;
    Vector3 leftHandPosition;
    Vector3 rightHandPosition;
    Vector3 rightFootPosition;
    //Get Set
    #region
    public CharacterController Ch { get { return ch; } }
    public bool IsSprintPressed { get { return isSprintPressed; }}
    public bool IsMovePressed { get { return isMovementPressed; } }
    public float Speed { get { return maximumSpeed; } set { maximumSpeed = value; } }
    public float SprintSpeed { get { return sprintSpeed; } set { sprintSpeed = value; } }
    public float VerticalVelocity { get { return _yspeed; } set { _yspeed = value; } }
    public float StandThreshold { get { return standThreshold; } set { standThreshold = value; } }   
    public int SpeedstateHash { get { return playerstateHash; } }
    public int MoveboolHash { get { return moveboolHash;} }
    public int MoveSpeedHash { get { return moveSpeedHash; } }
    public int VerticalVelHash { get { return verticalVelHash; } } 
    public Vector3 CurrentMovement { get { return currentMovemevt; } }
    #endregion



    private void Awake()
    {
        ch = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        _input = new PlayerInput1();
        _input.CharacterControl.Movement.started += onMovementInput;
        _input.CharacterControl.Movement.performed += onMovementInput;
        _input.CharacterControl.Movement.canceled += onMovementInput;
        _input.CharacterControl.Sprinting.started += onSprint;
        _input.CharacterControl.Sprinting.canceled += onSprint;
        _input.CharacterControl.Jump.started += onJump;
        _input.CharacterControl.Jump.canceled += onJump;

        setupJumpVariables();
    }
    //InputSystem
    #region
    private void onJump(InputAction.CallbackContext jump)
    {
        isJumpPressed = jump.ReadValueAsButton();
    }

    private void onSprint(InputAction.CallbackContext sprint)
    {
        isSprintPressed = sprint.ReadValueAsButton();
    }

    private void onMovementInput(InputAction.CallbackContext move)
    {
        currentMovemevtInput = move.ReadValue<Vector2>();
        currentMovemevt.x = currentMovemevtInput.x;
        currentMovemevt.z = currentMovemevtInput.y;
        isMovementPressed = currentMovemevtInput.x != 0 || currentMovemevtInput.y != 0;
    }
    #endregion
    void SwitchPlayerStates()
    {
        switch (playerPosture)
        {
            case PlayerPosture.Stand:
                if (!isGrounded)
                {
                    if (VerticalVelocity > 0)
                    {
                        playerPosture = PlayerPosture.Jumping;
                    }
                    else if (couldFall)
                    {
                        playerPosture = PlayerPosture.Falling;
                    }
                }
                else if (isClimbReady)
                {
                    playerPosture = PlayerPosture.Climbing;
                }
                isClimbReady = false;
                break;
            case PlayerPosture.Falling:
                if (isGrounded)
                {
                    StartCoroutine(CoolDownJump());
                }
                if (isLanding)
                {
                    playerPosture = PlayerPosture.Landing;
                }
                isClimbReady = false;
                break;

            case PlayerPosture.Jumping:
                if (isGrounded)
                {
                    StartCoroutine(CoolDownJump());
                }
                if (isLanding)
                {
                    playerPosture = PlayerPosture.Landing;
                }
                isClimbReady = false;
                break;

            case PlayerPosture.Landing:
                if (!isLanding)
                {
                    playerPosture = PlayerPosture.Stand;
                }
                isClimbReady = false;
                break;
            case PlayerPosture.Climbing:

                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("climb") && !animator.IsInTransition(0))
                {
                    playerPosture = PlayerPosture.Stand;
                }
                break;
        }
        #region
        //if (!isGrounded)
        //{
        //    if (_yspeed > 0)
        //    {
        //        playerPosture = PlayerPosture.Jumping;
        //    }
        //    else if (playerPosture != PlayerPosture.Jumping)
        //    {
        //        if (couldFall)
        //        {
        //            playerPosture = PlayerPosture.Falling;
        //        }
        //    }
        //}
        //else if (playerPosture == PlayerPosture.Jumping)
        //{
        //    StartCoroutine(CoolDownJump());
        //}
        //else if (isLanding)
        //{
        //    playerPosture = PlayerPosture.Landing;
        //}
        //else
        //{
        //    playerPosture = PlayerPosture.Stand;
        //}
        #endregion
        if (currentMovemevtInput.magnitude == 0)
        {
            locomotionState = LocomotionState.Idle;
        }
        else
        {
            locomotionState = LocomotionState.Move;
        }
    }
    void setupJumpVariables()
    {
        float timeToApex = maxJumpHeight / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }
    private void HandleJump()
    {

        if (playerPosture == PlayerPosture.Stand)
        {
            float velOffset;
            switch (locomotionState)
            {
                case LocomotionState.Move:
                    velOffset = isSprintPressed ? 1 : 0.5f;
                    break;
                case LocomotionState.Idle:
                    velOffset = 0f;
                    break;
                default:
                    velOffset = 0f;
                    break;
            }
            switch (playerSensor.ClimbDetection(playerTransform, currentMovemevtWorldSpace, velOffset))
            {
                case PlayerSensor.NextPlayerMovement.jump:
                    if(isJumpPressed)
                    {
                        //VerticalVelocity = initialJumpVelocity;
                        rb.velocity = Vector3.up * jumpForce;
                        feetTween = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
                        feetTween = feetTween < 0.5f ? 1 : -1;
                        if (locomotionState == LocomotionState.Move)
                        {
                            if (isSprintPressed)
                            {
                                feetTween *= 3;
                            }
                            else if (isMovementPressed)
                            {
                                feetTween *= 2;
                            }
                        }
                        else
                        {
                            feetTween = UnityEngine.Random.Range(-0.5f, 1f) * feetTween;
                        }
                    }
                    break;
                case PlayerSensor.NextPlayerMovement.climbLow:
                    Vector3 velocity = ch.velocity;
                    Vector3 forwardDirection = transform.forward;
                    float dotProduct = Vector3.Dot(velocity.normalized, forwardDirection);
                    if (dotProduct > 0)
                    {
                        //左手的位置向左移0.3米
                        leftHandPosition = playerSensor.Ledge + Vector3.Cross(-playerSensor.ClimbHitNormal, Vector3.up) * 0.3f;
                        currentClimbparameter = lowClimbParameter;
                        isClimbReady = true;
                    }else if(dotProduct < 0)
                    {
                        isClimbReady = false;
                    }else
                    {
                        isClimbReady = false;
                    }
                    break;
                case PlayerSensor.NextPlayerMovement.climbHigh:
                    if (isJumpPressed)
                    {
                        //右手的位置處於ledge右邊0.3米
                        rightHandPosition = playerSensor.Ledge + Vector3.Cross(playerSensor.ClimbHitNormal, Vector3.up) * 0.3f;
                        //腳的位置在頂端以下1.2米
                        rightFootPosition = playerSensor.Ledge + Vector3.down * 1.2f;
                        currentClimbparameter = highClimbParameter;
                        isClimbReady = true;
                    }                    
                    break;
                case PlayerSensor.NextPlayerMovement.vault:
                    currentClimbparameter = vaultParameter;
                    isClimbReady = true;
                    break;
            }
            //_yspeed = initialJumpVelocity;
            //feetTween = Mathf.Repeat(animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
            //feetTween = feetTween < 0.5f ? 1 : -1;
            //if(locomotionState == LocomotionState.Move)
            //{
            //    if (isSprintPressed)
            //    {
            //        feetTween *= 3;
            //    }else if (isMovementPressed)
            //    {
            //        feetTween *= 2;
            //    }
            //}
            //else
            //{
            //    feetTween = UnityEngine.Random.Range(-0.5f, 1f) * feetTween;
            //}
        }
    }
    IEnumerator CoolDownJump()
    {
        landingThreshold = Mathf.Clamp(VerticalVelocity, -10, 0);
        landingThreshold /= 20f;
        landingThreshold += 1f;
        isLanding = true;
        playerPosture = PlayerPosture.Landing;
        yield return new WaitForSeconds(jumpCD);
        isLanding = false;
    }
    void CheckGround()
    {
        if (Physics.SphereCast(playerTransform.position + (Vector3.up * groundCheckOffset), collider.radius, Vector3.down, out RaycastHit hit, groundCheckOffset - collider.radius + 2 * 0.02f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            couldFall = !Physics.Raycast(playerTransform.position, Vector3.down, fallHeight);
        }
    }
    private void FixedUpdate()
    {
        /*MovePlayer();*/
        LookAt();
    }
    /*private void MovePlayer()
    {
        float targetSpeed = isSprintPressed ? sprintSpeed : maximumSpeed;
        if (!isMovementPressed) targetSpeed = 0.0f;
        float currentHorizontalSpeed = new Vector3(ch.velocity.x, 0.0f, ch.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = Mathf.Clamp01(currentMovemevtInput.magnitude);

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * rotatespeed);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }
    }*/

    Vector3 AverageVel(Vector3 newVel)
    {
        velCache[currentChacheIndex] = newVel;
        currentChacheIndex++;
        currentChacheIndex %= CACHE_SIZE;
        Vector3 average = Vector3.zero;
        foreach (Vector3 vel in velCache)
        {
            average += vel;
        }
        return average / CACHE_SIZE;
    }
    private void OnAnimatorMove()
    {
        if (playerPosture == PlayerPosture.Climbing)
        {
            //ch.enabled = false;
            animator.ApplyBuiltinRootMotion();
        }
        else if (playerPosture != PlayerPosture.Jumping && playerPosture != PlayerPosture.Falling)
        {
            //ch.enabled = true;
            float targetSpeed = isSprintPressed ? sprintSpeed : maximumSpeed;
            Vector3 targetDirection = animator.deltaPosition;
            targetDirection.y = 0.0f;

            //rb.MovePosition(rb.position + (targetDirection * targetSpeed));
            rb.velocity = targetDirection / Time.deltaTime * targetSpeed;
            //ch.Move(targetDirection * targetSpeed);
            averageVel = AverageVel(animator.velocity);
        }
        else
        {
            //ch.enabled = true;
            float targetSpeed = isSprintPressed ? sprintSpeed : maximumSpeed;
            averageVel.y = 0.0f;
            Vector3 targetDirection = averageVel * Time.deltaTime;
            rb.velocity = averageVel * targetSpeed;
            //ch.Move(targetDirection * targetSpeed);
        }
    }
    private void LookAt()
    {
        if(playerPosture != PlayerPosture.Jumping)
        {
            Vector3 inputDirection = new Vector3(currentMovemevtInput.x, 0.0f, currentMovemevtInput.y).normalized;
            if (currentMovemevtInput != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  playerCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }
    }
    void CaculateInputDirection()
    {
        Vector3 camForwardProjection = new Vector3(playerCamera.transform.forward.x, 0, playerCamera.transform.forward.z).normalized;
        currentMovemevtWorldSpace = camForwardProjection * currentMovemevtInput.y + playerCamera.transform.right * currentMovemevtInput.x;
        currentMovemevt = playerTransform.InverseTransformVector(currentMovemevtWorldSpace);
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerTransform = transform;
        playerSensor = GetComponent<PlayerSensor>();
        playerstateHash = Animator.StringToHash("playerstate");
        midairHash = Animator.StringToHash("midairstate");
        moveSpeedHash = Animator.StringToHash("movestate");
        moveboolHash = Animator.StringToHash("isMoving");
        jumpboolHash = Animator.StringToHash("isJump");

        verticalVelHash = Animator.StringToHash("verticalspeed");
        feetTweenHash = Animator.StringToHash("feet");
    }
    void Update()
    {
        HandleGravity();
        HandleMovementAnimation();
        CheckGround();
        HandleJump();
        CaculateInputDirection();
        SwitchPlayerStates();
    }

    private void HandleGravity()
    {
        if (playerPosture != PlayerPosture.Jumping && playerPosture != PlayerPosture.Falling)
        {
            if (!isGrounded)
            {
                VerticalVelocity += gravity * fallMultiplier * Time.deltaTime;
            }
            else
            {
                VerticalVelocity = groundedGravity;
            }
        }
        else
        {
            if (VerticalVelocity <= 0 || !isJumpPressed)
            {
                float previousYVelocity = VerticalVelocity;
                float newYVelocity = VerticalVelocity + (gravity * fallMultiplier * Time.deltaTime);
                float nextYVelocity = Math.Max((previousYVelocity + newYVelocity) * .5f, -20.0f);
                VerticalVelocity = nextYVelocity;
            }
            else
            {
                float previousYVelocity = VerticalVelocity;
                float newYVelocity = VerticalVelocity + (gravity * Time.deltaTime);
                float nextYVelocity = Math.Max((previousYVelocity + newYVelocity) * .5f, -20.0f);
                VerticalVelocity = nextYVelocity;
            }
        }
    }
    private void HandleMovementAnimation()
    {
        if (playerPosture == PlayerPosture.Stand)
        {
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetBool(moveboolHash, false);
                    animator.SetBool(jumpboolHash, false);
                    break;
                case LocomotionState.Move:
                    animator.SetBool(moveboolHash, true);
                    animator.SetBool(jumpboolHash, false);
                    animator.SetFloat(playerstateHash, standThreshold, 0.1f, Time.deltaTime);
                    break;
            }
        }
        else if (playerPosture == PlayerPosture.Jumping)
        {
            animator.SetBool(moveboolHash, true);
            animator.SetFloat(playerstateHash, jumpThreshold);
            animator.SetFloat(verticalVelHash, _yspeed);
            animator.SetFloat(feetTweenHash, feetTween);
        }
        else if (playerPosture == PlayerPosture.Landing)
        {
            switch (locomotionState)
            {
                case LocomotionState.Idle:
                    animator.SetBool(moveboolHash, false);
                    animator.SetBool(jumpboolHash, false);
                    break;
                case LocomotionState.Move:
                    animator.SetBool(moveboolHash, true);
                    animator.SetBool(jumpboolHash, false);
                    animator.SetFloat(playerstateHash, landingThreshold, 0.03f, Time.deltaTime);
                    break;
            }
        }else if(playerPosture == PlayerPosture.Falling)
        {
            animator.SetFloat(playerstateHash, jumpThreshold);
            animator.SetFloat(verticalVelHash, _yspeed);
        }else if(playerPosture == PlayerPosture.Climbing)
        {
            animator.SetInteger("climb", currentClimbparameter);
            playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, Quaternion.LookRotation(-playerSensor.ClimbHitNormal), 0.5f);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Climb low"))
            {
                currentClimbparameter = defaultClimbParameter;
                if (animator.IsInTransition(layerMask))
                    return;
                animator.MatchTarget(leftHandPosition, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.one, 0f), 0f, 0.1f);
                animator.MatchTarget(leftHandPosition + Vector3.up * 0.18f, Quaternion.identity, AvatarTarget.LeftHand, new MatchTargetWeightMask(Vector3.up, 0f), 0.1f, 0.3f);
            }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Climb high"))
            {
                currentClimbparameter = defaultClimbParameter;
                if (animator.IsInTransition(layerMask))
                    return;
                animator.MatchTarget(rightFootPosition, Quaternion.identity, AvatarTarget.RightFoot, new MatchTargetWeightMask(Vector3.one, 0f), 0f, 0.14f);
                animator.MatchTarget(rightHandPosition, Quaternion.identity, AvatarTarget.RightHand, new MatchTargetWeightMask(Vector3.one, 0f), 0.2f, 0.32f);
            }           
        }
    }
    private void OnEnable()
    {
        //move = _input.CharacterControl.Movement;
        _input.CharacterControl.Enable();
    }
    private void OnDisable()
    {
        _input.CharacterControl.Disable();
    }
}

