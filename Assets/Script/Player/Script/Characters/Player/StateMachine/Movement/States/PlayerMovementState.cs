using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Nomimovment
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine stateMachine;
        protected PlayerGroundData movementData;
        protected PlayerAirborneData airborneData;
        #region 
        static readonly int CACHE_SIZE = 3;
        Vector3[] velCache = new Vector3[CACHE_SIZE];
        int currentChacheIndex = 0;
        Vector3 averageVel = Vector3.zero;
        #endregion
        public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            stateMachine = playerMovementStateMachine;
            movementData = stateMachine.Player.Data.GroundedData;
            airborneData = stateMachine.Player.Data.AirborneData;

            SetBaseCameraRecenteringData();

            InitializedData();
        }


        private void InitializedData()
        {
            SetBaseRotationData();
        }

        #region IState Methods
        public virtual void Enter()
        {
            Debug.Log("State: " + GetType().Name);
            AddInputActionCallback();
        }
        public virtual void Exit()
        {
            RemoveInputActionCallback();
        }
        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void Update()
        {
            CurrentMovement();
            CheckGround();
            Move();
        }
        public virtual void PhysicsUpdate()
        {
            LookAt();
            OnAnimatonMove();
        }
        public virtual void OnAnimationMove()
        {
        }
        public virtual void OnAnimationEnterEvent()
        {
        }
        public virtual void OnAnimationExitEvent()
        {
        }
        public virtual void OnAnimationTransitionEvent()
        {
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);
                return;
            }
            if (stateMachine.Player.LayerData.IsLedgeLayer(collider.gameObject.layer))
            {
                OnContactWithLedge(collider);
                return;
            }
        }

        public void OnTriggerExit(Collider collider)
        {
            if (stateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGroundExit(collider);
                return;
            }
            if (stateMachine.Player.LayerData.IsLedgeLayer(collider.gameObject.layer))
            {
                OnContactWithLedgeExit(collider);
                return;
            }
        }
        #endregion
        #region Main Methods
        private void ReadMovementInput()
        {
            stateMachine.ReusableData.CurrentMovementInput = stateMachine.Player.Inputs.PlayerActions.Movement.ReadValue<Vector2>();
        }
        private void Move()
        {
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero || stateMachine.ReusableData.SpeedMultiplier == 0.0f || !stateMachine.ReusableData.IsGrounded || stateMachine.ReusableData.IsSliding)
            {
                return;
            }
            Vector3 movementDirection = GetMovementDirection();
            Rotate(movementDirection);
            movementDirection.Normalize();
            //Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
        }
        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);

            LookAt();

            return directionAngle;
        }
        private float GetDirectionAngle(Vector3 direction)
        {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (directionAngle < 0f)
            {
                directionAngle += 360f;
            }
            return directionAngle;
        }
        private float AddCameraRotationToAngle(float angle)
        {
            angle += stateMachine.Player.playerCamera.transform.eulerAngles.y;

            if (angle > 360f)
            {
                angle -= 360f;
            }

            return angle;
        }
        private void UpdateTargetRotationData(float tagetAngle)
        {
            stateMachine.ReusableData.currenttargetRotation = tagetAngle;
        }

        private void OnAnimatonMove()
        {
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero || stateMachine.ReusableData.SpeedMultiplier == 0.0f || stateMachine.ReusableData.IsSliding)
            {
                return;
            }
            float movementSpeed = GetMovementSpeed();
            Vector3 movementDirection = stateMachine.Player.Animator.deltaPosition;
            movementDirection.y = 0.0f;
            stateMachine.Player.Rigidbody.velocity = movementDirection / Time.deltaTime * movementSpeed;
            //stateMachine.Player.CharacterController.Move(movementDirection * movementSpeed);
            averageVel = AverageVel(stateMachine.Player.Rigidbody.velocity);
        }
        #endregion
        #region Reusable Methods
        protected void SetBaseCameraRecenteringData()
        {
            stateMachine.ReusableData.BackwardsCameraRecenteringData = movementData.BackwardsCameraRecenteringData;
            stateMachine.ReusableData.SidewaysCameraRecenteringData = movementData.SidewaysCameraRecenteringData;
        }
        protected void SetBaseRotationData()
        {
            stateMachine.ReusableData.RotatonData = movementData.BaseRotationData;
            stateMachine.ReusableData.RotationSmoothTime = stateMachine.ReusableData.RotatonData.RotationSmoothTime;
        }
        protected void StartAnimation(int animationHash)
        {
            stateMachine.Player.Animator.SetBool(animationHash, true);
        }
        protected void StopAnimation(int animationHash)
        {
            stateMachine.Player.Animator.SetBool(animationHash, false);
        }
        protected void AnimationFloat(int animationHash, float stateThreshold, float speed, float time)
        {
            stateMachine.Player.Animator.SetFloat(animationHash, stateThreshold, speed, time);
        }
        protected Vector3 GetMovementDirection()
        {
            return new Vector3(stateMachine.ReusableData.CurrentMovementInput.x, 0.0f, stateMachine.ReusableData.CurrentMovementInput.y);

        }
        protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
        {
            float movementspeed = movementData.BaseSpeed * stateMachine.ReusableData.SpeedMultiplier;
            if (shouldConsiderSlopes)
            {
                movementspeed *= stateMachine.ReusableData.SlopeSpeedMultiplier;
            }
            return movementspeed;
        }
        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = stateMachine.Player.Rigidbody.velocity;

            playerHorizontalVelocity.y = 0f;

            return playerHorizontalVelocity;
        }
        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, stateMachine.Player.Rigidbody.velocity.y, 0);
        }
        protected Vector3 GetAverageVel()
        {
            return averageVel;
        }
        protected void LookAt()
        {
            float currentAngle = stateMachine.Player.playerTransform.transform.eulerAngles.y;
            if (currentAngle == stateMachine.ReusableData.currenttargetRotation)
            {
                return;
            }
            float rotation = Mathf.SmoothDampAngle(currentAngle, stateMachine.ReusableData.currenttargetRotation, ref stateMachine.ReusableData.RotationVelocity.y, stateMachine.ReusableData.RotationSmoothTime);
            Quaternion targetRotation = Quaternion.Euler(0.0f, rotation, 0.0f);

            stateMachine.Player.playerTransform.rotation = targetRotation;
        }
        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            float directionAngle = GetDirectionAngle(direction);

            if (shouldConsiderCameraRotation)
            {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }

            if (directionAngle != stateMachine.ReusableData.currenttargetRotation)
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }
        protected Vector3 GetTargetRotationDirection(float tagetAngle)
        {
            return Quaternion.Euler(0.0f, tagetAngle, 0.0f) * Vector3.forward;
        }
        protected Vector3 AverageVel(Vector3 newVel)
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
        protected void UpdateCameraRecenteringState(Vector2 movementInput)
        {
            if (movementInput == Vector2.zero)
            {
                return;
            }

            if (movementInput == Vector2.up)
            {
                DisableCameraRecentering();

                return;
            }

            float cameraVerticalAngle = stateMachine.Player.playerCamera.eulerAngles.x;

            if (cameraVerticalAngle >= 270f)
            {
                cameraVerticalAngle -= 360f;
            }

            cameraVerticalAngle = Mathf.Abs(cameraVerticalAngle);

            if (movementInput == Vector2.down)
            {
                SetCameraRecenteringState(cameraVerticalAngle, stateMachine.ReusableData.BackwardsCameraRecenteringData);

                return;
            }

            SetCameraRecenteringState(cameraVerticalAngle, stateMachine.ReusableData.SidewaysCameraRecenteringData);
        }
        protected void SetCameraRecenteringState(float cameraVerticalAngle, List<PlayerCameraRecenteringData> cameraRecenteringData)
        {

            foreach (PlayerCameraRecenteringData recenteringData in cameraRecenteringData)
            {
                if (!recenteringData.IsWithinRange(cameraVerticalAngle))
                {
                    continue;
                }

                EnableCameraRecentering(recenteringData.WaitTime, recenteringData.RecenteringTime);

                return;
            }

            DisableCameraRecentering();
        }
        protected void EnableCameraRecentering(float waitTime = -1f, float recenteringTime = -1f)
        {
            float movementSpeed = GetMovementSpeed();

            if (movementSpeed == 0f)
            {
                movementSpeed = movementData.BaseSpeed;
            }

            stateMachine.Player.CameraRecenteringUtility.EnableRecentering(waitTime, recenteringTime, movementData.BaseSpeed, movementSpeed);
        }
        protected void DisableCameraRecentering()
        {
            stateMachine.Player.CameraRecenteringUtility.DisableRecentering();
        }
        protected void ResetVelocity()
        {
            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
            stateMachine.Player.Rigidbody.velocity = Vector3.zero;
            //Vector3 movementDirection = Vector3.zero;
            //stateMachine.Player.CharacterController.Move(movementDirection);
        }
        protected void ResetVerticalVelocity()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            stateMachine.Player.Rigidbody.velocity = playerHorizontalVelocity;
        }

        protected virtual void AddInputActionCallback()
        {
            stateMachine.Player.Inputs.PlayerActions.WalkToggle.started += OnWalkToggleStarted;

            stateMachine.Player.Inputs.PlayerActions.Look.started += OnMouseMovementStarted;

            stateMachine.Player.Inputs.PlayerActions.Movement.performed += OnMovementPerformed;

            stateMachine.Player.Inputs.PlayerActions.Movement.canceled += OnMovementCanceled;
        }

        protected virtual void RemoveInputActionCallback()
        {
            stateMachine.Player.Inputs.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;

            stateMachine.Player.Inputs.PlayerActions.Look.started -= OnMouseMovementStarted;

            stateMachine.Player.Inputs.PlayerActions.Movement.performed -= OnMovementPerformed;

            stateMachine.Player.Inputs.PlayerActions.Movement.canceled -= OnMovementCanceled;
        }
        protected void DelcelerateHorizontally()
        {
            Vector3 StopHorizontallyVelocity = stateMachine.Player.Animator.deltaPosition;
            stateMachine.Player.Rigidbody.velocity = StopHorizontallyVelocity / Time.deltaTime;
            //stateMachine.Player.CharacterController.Move(movementDirection);
        }
        protected void DelcelerateVertically()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();
            stateMachine.Player.Rigidbody.AddForce(-playerVerticalVelocity * stateMachine.ReusableData.MovementDelcelerationForce, ForceMode.Acceleration);
            //stateMachine.Player.CharacterController.Move(movementDirection);
        }
        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 playerHorizontaVelocity = GetPlayerHorizontalVelocity();

            Vector2 playerHorizontalMovement = new Vector2(playerHorizontaVelocity.x, playerHorizontaVelocity.z);

            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }
        protected bool IsMovingUp(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y > minimumVelocity;
        }
        protected bool IsMovingDown(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y < -minimumVelocity;
        }
        protected virtual void OnContactWithGround(Collider collider)
        {
        }
        protected virtual void OnContactWithLedge(Collider collider)
        {
        }
        protected virtual void OnContactWithGroundExit(Collider collider)
        {
        }
        protected virtual void OnContactWithLedgeExit(Collider collider)
        {
        }
        protected virtual void CurrentMovement()
        {
            stateMachine.ReusableData.CurrentMovement.x = stateMachine.ReusableData.CurrentMovementInput.x;
            stateMachine.ReusableData.CurrentMovement.z = stateMachine.ReusableData.CurrentMovementInput.y;
        }
        protected bool CheckGround()
        {
            if (Physics.SphereCast(stateMachine.Player.playerTransform.position + (Vector3.up * stateMachine.ReusableData.GroundCheckOffset)
                , stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.radius, Vector3.down, out RaycastHit hit
                , stateMachine.ReusableData.GroundCheckOffset - stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.radius + 2 * stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.skinWidth
                , stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                stateMachine.ReusableData.IsGrounded = true;
                return stateMachine.ReusableData.IsGrounded;
            }
            else
            {
                stateMachine.ReusableData.IsGrounded = false;
                return stateMachine.ReusableData.IsGrounded;
            }
        }
        #endregion
        #region Input Methods
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            stateMachine.ReusableData.ShouldWalk = !stateMachine.ReusableData.ShouldWalk;
        }
        private void OnMouseMovementStarted(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(stateMachine.ReusableData.CurrentMovementInput);
        }
        private void OnMovementPerformed(InputAction.CallbackContext context)
        {
            UpdateCameraRecenteringState(context.ReadValue<Vector2>());
        }
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            DisableCameraRecentering();

        }
        #endregion
    }
}


