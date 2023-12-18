using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
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
            //CheckGround();
            RaycastFallDown();
            FaceingWall();
            LedgeRaycast();
            Move();
        }
        public virtual void PhysicsUpdate()
        {
            LookAt();
        }


        public virtual void OnAnimationMove()
        {
            OnAnimatonMove();
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
            if (stateMachine.Player.LayerData.IsLedgeLayer(collider.gameObject.layer))
            {
                OnContactWithLedge(collider);
                return;
            }
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            if (stateMachine.Player.LayerData.IsLedgeLayer(collider.gameObject.layer))
            {
                OnContactExitWithLedge(collider);
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
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero || stateMachine.ReusableData.SpeedMultiplier == 0.0f || !IsGround() || stateMachine.ReusableData.IsSliding || stateMachine.ReusableData.OnLedge && stateMachine.ReusableData.FaceWall)
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
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero || stateMachine.ReusableData.SpeedMultiplier == 0.0f || stateMachine.ReusableData.IsSliding || !IsGround() || stateMachine.ReusableData.OnLedge && stateMachine.ReusableData.FaceWall)
            {
                return;
            }
            float movementSpeed = GetMovementSpeed();
            Vector3 movementDirection = stateMachine.Player.Animator.deltaPosition;
            movementDirection.y = 0.0f;
            stateMachine.Player.CharacterController.Move(movementDirection * movementSpeed);
            averageVel = AverageVel(stateMachine.Player.Animator.velocity);
        }
        #endregion
        #region Reusable Methods
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
        protected float GetMovementSpeed()
        {
            return movementData.BaseSpeed * stateMachine.ReusableData.SpeedMultiplier;
        }
        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, stateMachine.ReusableData.VerticalVelocity, 0);
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
        protected void ResetVelocity()
        {
            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
            Vector3 movementDirection = Vector3.zero;
            stateMachine.Player.CharacterController.Move(movementDirection);
        }
        protected virtual void AddInputActionCallback()
        {
            stateMachine.Player.Inputs.PlayerActions.WalkToggle.started += OnWalkToggleStarted;
        }
        protected virtual void RemoveInputActionCallback()
        {
            stateMachine.Player.Inputs.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;
        }
        protected void DelcelerateHorizontally()
        {
            Vector3 movementDirection = stateMachine.Player.Animator.deltaPosition;
            movementDirection.y = 0.0f * Time.deltaTime;
            stateMachine.Player.CharacterController.Move(movementDirection);
        }
        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 playerHorizontaVelocity = stateMachine.Player.CharacterController.velocity;

            Vector2 playerHorizontalMovement = new Vector2(playerHorizontaVelocity.x, playerHorizontaVelocity.z);

            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }
        protected virtual void CurrentMovement()
        {
            stateMachine.ReusableData.CurrentMovement.x = stateMachine.ReusableData.CurrentMovementInput.x;
            stateMachine.ReusableData.CurrentMovement.z = stateMachine.ReusableData.CurrentMovementInput.y;
        }
        protected virtual void OnContactWithLedge(Collider collider)
        {
        }
        protected virtual void OnContactExitWithLedge(Collider collider)
        {
        }
        protected void FaceingWall()
        {
            Vector3 rayStrat;
            RaycastHit hit;

            if (Physics.Raycast(stateMachine.Player.TestingData.wallTransform.position, stateMachine.Player.playerTransform.forward, out hit, 2f))
            {
                rayStrat = hit.point;
                rayStrat.y -= 1.0f;
                stateMachine.ReusableData.FaceWall = true;
                if (Physics.Raycast(rayStrat, stateMachine.Player.playerTransform.forward, out RaycastHit wallhit, 2f, stateMachine.Player.LayerData.WallLayer))
                {
                    if (stateMachine.ReusableData.OnLedge)
                    {
                        stateMachine.Player.playerTransform.forward = -wallhit.normal;
                    }
                }
                Vector3 _forward = stateMachine.Player.playerTransform.TransformDirection(Vector3.forward) * 1.0f;
                Debug.DrawRay(rayStrat, _forward, Color.green, 2f);
            }
            else
            {
                stateMachine.ReusableData.FaceWall = false;
            }
            //Vector3 _forward2 = stateMachine.Player.playerTransform.TransformDirection(Vector3.forward) * 1.0f;
            //Debug.DrawRay(stateMachine.Player.TestingData.wallTransform.position, _forward2, Color.green, 2f);
        }

        protected void LedgeRaycast()
        {
            int numberOfRays = 10;
            for( int i = 0; i < numberOfRays; i++)
            {
                var climbOrign = stateMachine.Player.playerTransform.position + Vector3.up * 1.2f;
                var climbOffect = new Vector3(0, 0.3f, 0);
                Vector3 rayStart;
                RaycastHit rayhitwall;
                if (Physics.Raycast(climbOrign + climbOffect * i, stateMachine.Player.playerTransform.forward, out rayhitwall, 2f, stateMachine.Player.LayerData.WallLayer))
                {
                    Debug.DrawRay(climbOrign + climbOffect * i, stateMachine.Player.playerTransform.forward , Color.white, 2f);
                    rayStart = rayhitwall.point + stateMachine.Player.playerTransform.forward * 0.03f;
                    rayStart.y += 3.0f;
                    if (Physics.Raycast(rayStart, Vector3.down, out stateMachine.ReusableData.rayFindLedge, 4f))
                    {
                        Vector3 ledgemarker;
                        ledgemarker = new Vector3(rayhitwall.transform.position.x, stateMachine.ReusableData.rayFindLedge.transform.position.y, rayhitwall.transform.position.z);
                        GameObject TemporayMark;
                        TemporayMark = Player.Instantiate(stateMachine.Player.TestingData.rayhitmark, stateMachine.ReusableData.rayFindLedge.point, Quaternion.LookRotation(stateMachine.ReusableData.rayFindLedge.normal)) as GameObject;
                        Player.Destroy(TemporayMark, 0.03f);
                    }
                    Vector3 _down = stateMachine.Player.transform.TransformDirection(Vector3.down) * 5;
                    Debug.DrawRay(rayStart, _down, Color.red, 4f);
                }
            }
        }

        protected bool RaycastFallDown()
        {
            int numberOfRays = 3;
            for (int i = 0; i < numberOfRays; i++)
            {
                var origin = stateMachine.Player.playerTransform.position + Vector3.down * 0.15f + stateMachine.Player.transform.forward * 1f;
                var originOffect = new Vector3(0, -0.15f, 0);

                if (Physics.Raycast(origin + originOffect * i, -stateMachine.Player.transform.forward, out RaycastHit hit, 3))
                {
                    Debug.DrawRay(origin + originOffect * i, -stateMachine.Player.transform.forward, Color.blue, 3f);
                    return true;
                }
            }
            return false;
        }
        protected bool IsGround()
        {
            return Physics.CheckSphere(stateMachine.Player.playerTransform.position, .1f, stateMachine.Player.LayerData.GroundLayer);
        }


        //protected virtual void CheckGround()
        //{
        //    if (Physics.SphereCast(stateMachine.Player.playerTransform.position + (Vector3.up * stateMachine.ReusableData.GroundCheckOffset)
        //        , stateMachine.Player.CharacterController.radius, Vector3.down, out RaycastHit hit
        //        , stateMachine.ReusableData.GroundCheckOffset - stateMachine.Player.CharacterController.radius + 2 * stateMachine.Player.CharacterController.skinWidth
        //        , stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        //    {
        //        stateMachine.ReusableData.IsGrounded = true;
        //        Debug.Log("isGround");
        //    }
        //    else
        //    {
        //        stateMachine.ReusableData.IsGrounded = false;
        //        Debug.Log("is NOT Ground");
        //    }
        //}
        #endregion
        #region Input Methods
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            stateMachine.ReusableData.ShouldWalk = !stateMachine.ReusableData.ShouldWalk;
        }
        #endregion

    }
}



