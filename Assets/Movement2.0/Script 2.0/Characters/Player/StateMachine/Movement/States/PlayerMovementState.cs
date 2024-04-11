using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine stateMachine;
        protected PlayerGroundData movementData;
        protected PlayerAirborneData airborneData;

        private float starttime;
        private float startfix;
        public float slowdownFactor = 0.05f;
        GameObject starobj;
        bool mouseInput;


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
            starttime = Time.timeScale;
            startfix = Time.fixedDeltaTime;
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
            FaceingWall();
            //CheckGround();
            RaycastFallDown();
            Move();
        }
        public virtual void PhysicsUpdate()
        {
            RotateTowardsTargetRotion();
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
        }

        //public void OnTriggerStay(Collider collider)
        //{
        //    if (stateMachine.Player.LayerData.IsStarLayer(collider.gameObject.layer))
        //    {
        //        OnContactInStar(collider);
        //        return;
        //    }
        //}

        //public virtual void OnTriggerExit(Collider collider)
        //{
        //    if (stateMachine.Player.LayerData.IsStarLayer(collider.gameObject.layer))
        //    {
        //        OnContactExitWithStar(collider);
        //        return;
        //    }
        //}
        #endregion
        #region Main Methods
        private void ReadMovementInput()
        {
            stateMachine.ReusableData.CurrentMovementInput = stateMachine.Player.Inputs.PlayerActions.Movement.ReadValue<Vector2>();
        }
        private void Move()
        {
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero || stateMachine.ReusableData.SpeedMultiplier == 0.0f || stateMachine.ReusableData.IsSliding)
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

            RotateTowardsTargetRotion();

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
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero || stateMachine.ReusableData.SpeedMultiplier == 0.0f)
            {
                return;
            }
            float movementSpeed = GetMovementSpeed();
            Vector3 movementDirection = stateMachine.Player.Animator.deltaPosition;
            movementDirection = AdjustVelocityToSlope(movementDirection);
            movementDirection.y += stateMachine.ReusableData.VerticalVelocity;
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
            //return new Vector3(0f, stateMachine.Player.Rigidbody.velocity.y, 0);
            return new Vector3(0f, stateMachine.ReusableData.VerticalVelocity, 0);
        }
        protected Vector3 GetAverageVel()
        {
            return averageVel;
        }
        protected void RotateTowardsTargetRotion()
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
        protected Vector3 AdjustVelocityToSlope(Vector3 velocity)
        {
            var ray = new Ray(stateMachine.Player.playerTransform.position, Vector3.down);
            if(Physics.Raycast(ray, out RaycastHit hit, 0.2f))
            {
                var slopeRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                var adjustedVelocity = slopeRotation * velocity;
                if(adjustedVelocity.y < 0)
                {
                    return adjustedVelocity;
                }
            }
            return velocity;
        }
        protected void ResetVelocity()
        {
            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
            stateMachine.Player.CharacterController.velocity.Set(0,0,0);
        }
        protected void ResetVerticalVelocity()
        {
            stateMachine.ReusableData.VerticalVelocity = 0.0f;
        }
        protected virtual void AddInputActionCallback()
        {
            stateMachine.Player.Inputs.PlayerActions.WalkToggle.started += OnWalkToggleStarted;
            stateMachine.Player.Inputs.PlayerActions.Suisei.started += OnSuiseiStrated;
            stateMachine.Player.Inputs.PlayerActions.Suisei.canceled += OnSuiSeiCanceled;

        }
        protected virtual void RemoveInputActionCallback()
        {
            stateMachine.Player.Inputs.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;

            stateMachine.Player.Inputs.PlayerActions.Suisei.started -= OnSuiseiStrated;
            stateMachine.Player.Inputs.PlayerActions.Suisei.started -= OnSuiseiStrated;
        }
        protected void DelcelerateHorizontally()
        {
            Vector3 movementDirection = stateMachine.Player.Animator.deltaPosition;
            //movementDirection.y = stateMachine.Player.Rigidbody.velocity.y;
            stateMachine.Player.CharacterController.Move(movementDirection);
        }
        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 playerHorizontaVelocity = stateMachine.Player.CharacterController.velocity;

            Vector2 playerHorizontalMovement = new Vector2(playerHorizontaVelocity.x, playerHorizontaVelocity.z);

            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }
        private void CurrentMovement()
        {
            stateMachine.ReusableData.CurrentMovement.x = stateMachine.ReusableData.CurrentMovementInput.x;
            stateMachine.ReusableData.CurrentMovement.z = stateMachine.ReusableData.CurrentMovementInput.y;
        }
        protected void Bash()
        {
            Collider[] Zones = Physics.OverlapSphere(stateMachine.Player.transform.position, 3f);
            foreach (Collider star in Zones)
            {
                stateMachine.ReusableData.NearStar = false;
                if (star.tag == "Star")
                {
                    stateMachine.ReusableData.NearStar = true;
                    starobj = star.transform.gameObject;
                    break;
                }
            }
            if (stateMachine.ReusableData.NearStar)
            {
                starobj.GetComponent<Renderer>().material.color = Color.yellow;
                if (mouseInput)
                {
                    starobj.GetComponent<Arrow>().arrow.SetActive(true);
                    LockCamera();
                    DoSlowmotion();
                    stateMachine.ReusableData.IscloseingDir = true;
                }
                else if (stateMachine.ReusableData.IscloseingDir && !mouseInput)
                {
                    starobj.GetComponent<Arrow>().arrow.SetActive(false);
                    stateMachine.Player.StartCoroutine(SuiSeiRoutine());
                    //starobj.GetComponent<Rigidbody>().AddForce(-stateMachine.ReusableData.SuiSeiJumpDir * 30f, ForceMode.Impulse);
                }
            }
            else if (starobj != null)
            {
                starobj.GetComponent<Renderer>().material.color = Color.red;
                starobj.GetComponent<Arrow>().arrow.SetActive(false);
                stateMachine.ReusableData.IscloseingDir = false;
                StopSlowmotion();
                UnLockCamera();
            }
        }
        protected IEnumerator SuiSeiRoutine()
        {
            UnLockCamera();
            StopSlowmotion();
            stateMachine.ReusableData.IscloseingDir = false;
            stateMachine.ReusableData.NearStar = false;
            ResetVerticalVelocity();
            stateMachine.Player.transform.position = starobj.transform.position;
            stateMachine.ReusableData.SuiSeiJumpDir = starobj.transform.forward;
            stateMachine.ReusableData.ShouldSuiseiJump = true;
            stateMachine.ReusableData.SuiSeiJumpDir.Normalize();
            yield return null;
            if (stateMachine.ReusableData.ShouldSuiseiJump)
            {
                //if (stateMachine.Player.CoolDownData.IsSuiSeiJumpCoolingDown) return;
                SuiseiJump();
            }
        }
        private void SuiseiJump()
        {
            stateMachine.ChangeState(stateMachine.SuiSeiJumpState);
        }
        //protected virtual void OnContactWithJumpPads(Collider collider)
        //{
        //}
        //protected virtual void OnContactExitWithStar(Collider collider)
        //{
        //}
        //protected virtual void OnContactInStar(Collider collider)
        //{
        //}
        protected void StopAllPartical()
        {
            stateMachine.Player.ParticalData.CameraParticle.Stop();
        }
        protected void LockCamera()
        {
            if (stateMachine.Player.cinemachineInput != null)
                stateMachine.Player.cinemachineInput.enabled = false;
        }
        protected void ShakeCamera()
        {
            if (stateMachine.Player.cinemachineImpulseSource != null)
                stateMachine.Player.cinemachineImpulseSource.GenerateImpulseWithForce(movementData.DashData.DashShackForce);
        }
        protected void UnLockCamera()
        {
            if (stateMachine.Player.cinemachineInput != null)
                stateMachine.Player.cinemachineInput.enabled = true;
        }
        protected void DoSlowmotion()
        {
            Time.timeScale = slowdownFactor;
            Time.fixedDeltaTime = Time.timeScale * .02f;
        }
        protected void StopSlowmotion()
        {
            Time.timeScale = starttime;
            Time.fixedDeltaTime = startfix;
        }
        protected void FaceingWall()
        {
            RaycastHit fwdHit;
            Vector3 FwdStart = new Vector3(stateMachine.Player.transform.position.x, stateMachine.ReusableData.rayFindLedge.point.y - 0.1f, stateMachine.Player.transform.position.z);
            Vector3 FwdEnd = new Vector3(stateMachine.Player.transform.position.x, stateMachine.ReusableData.rayFindLedge.point.y - 0.1f, stateMachine.Player.transform.position.z) + stateMachine.Player.transform.forward;
            Physics.Linecast(FwdStart, FwdEnd, out fwdHit, stateMachine.Player.LayerData.WallLayer);
            Debug.DrawLine(FwdStart, FwdEnd, Color.black);
            if (fwdHit.collider != null)
            {
                stateMachine.Player.transform.forward = -fwdHit.normal;
            }
        }

        //protected void LedgeRaycast()
        //{
        //    int numberOfRays = 10;
        //    for( int i = 0; i < numberOfRays; i++)
        //    {
        //        var climbOrign = stateMachine.Player.playerTransform.position + Vector3.up * 1.5f;
        //        var climbOffect = new Vector3(0, 0.3f, 0);
        //        Vector3 rayStart;
        //        RaycastHit rayhitwall;
        //        if (Physics.Raycast(climbOrign + climbOffect * i, stateMachine.Player.playerTransform.forward, out rayhitwall, 1f, stateMachine.Player.LayerData.WallLayer))
        //        {
        //            Debug.DrawRay(climbOrign + climbOffect * i, stateMachine.Player.playerTransform.forward , Color.white, 1f);
        //            rayStart = rayhitwall.point + stateMachine.Player.playerTransform.forward * 0.03f;
        //            rayStart.y += 3.0f;
        //            if (Physics.Raycast(rayStart, Vector3.down, out stateMachine.ReusableData.rayFindLedge, 3f))
        //            {
        //                Vector3 ledgemarker;
        //                ledgemarker = new Vector3(rayhitwall.transform.position.x, stateMachine.ReusableData.rayFindLedge.transform.position.y, rayhitwall.transform.position.z);
        //                GameObject TemporayMark;
        //                TemporayMark = Player.Instantiate(stateMachine.Player.TestingData.rayhitmark, stateMachine.ReusableData.rayFindLedge.point, Quaternion.LookRotation(stateMachine.ReusableData.rayFindLedge.normal)) as GameObject;
        //                Player.Destroy(TemporayMark, 0.03f);
        //            }
        //            Vector3 _down = stateMachine.Player.transform.TransformDirection(Vector3.down) * 5;
        //            Debug.DrawRay(rayStart, _down, Color.red, 3f);
        //        }
        //    }
        //}


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
            return Physics.Raycast(stateMachine.Player.playerTransform.position, Vector3.down, stateMachine.ReusableData.PlayerHeight * 0.5f + 0.3f, stateMachine.Player.LayerData.GroundLayer , QueryTriggerInteraction.Ignore);
        }
        protected bool IsMovingUp(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y > minimumVelocity;
        }

        protected bool IsMovingDown(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y < -minimumVelocity;
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
        private void OnSuiseiStrated(InputAction.CallbackContext context)
        {
            mouseInput = true;
        }
        private void OnSuiSeiCanceled(InputAction.CallbackContext context)
        {
            mouseInput = false;
        }

        #endregion

    }
}



