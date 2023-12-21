using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerAirborneState : PlayerMovementState
    {
        public PlayerAirborneState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            StopSlowmotion();
            UnLockCamera();
            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
            ResetSprintState();
            StartAnimation(stateMachine.Player.AnimationsData.AirstateHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopSlowmotion();
            UnLockCamera();
            stateMachine.ReusableData.ShouldAirDash = false;
            StopAnimation(stateMachine.Player.AnimationsData.AirstateHash);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            IsOnLedge();
            Bash();
            stateMachine.ReusableData.VerticalVelocity += stateMachine.ReusableData.Gravity * Time.deltaTime;
        }
        public override void Update()
        {
            base.Update();
            AnimationFloat(stateMachine.Player.AnimationsData.VerticalVelHash, stateMachine.ReusableData.VerticalVelocity / 7.5f, 0.2f, Time.deltaTime);
        }
        #endregion

        #region Main Methods

        protected void IsOnLedge()
        {
            Vector3 DownStart = (stateMachine.Player.transform.position + Vector3.up * 2.5f) + stateMachine.Player.transform.forward;
            Vector3 DownEnd = (stateMachine.Player.transform.position + Vector3.up * 0.7f) + stateMachine.Player.transform.forward;
            Physics.Linecast(DownStart, DownEnd, out stateMachine.ReusableData.rayFindLedge, stateMachine.Player.LayerData.WallLayer);
            Debug.DrawLine(DownStart, DownEnd);

            if (stateMachine.ReusableData.rayFindLedge.collider != null)
            {
                stateMachine.ReusableData.OnLedge = true;
                Vector3 FwdStart = new Vector3(stateMachine.Player.transform.position.x, stateMachine.ReusableData.rayFindLedge.point.y - 0.1f, stateMachine.Player.transform.position.z);
                Vector3 FwdEnd = new Vector3(stateMachine.Player.transform.position.x, stateMachine.ReusableData.rayFindLedge.point.y - 0.1f, stateMachine.Player.transform.position.z) + stateMachine.Player.transform.forward;
                Physics.Linecast(FwdStart, FwdEnd, out stateMachine.ReusableData.faceLedge, stateMachine.Player.LayerData.WallLayer);
                Debug.DrawLine(FwdStart, FwdEnd , Color.red);
                if (stateMachine.ReusableData.faceLedge.collider != null)
                {
                    if (stateMachine.Player.CoolDownData.IsClimbIdleingCoolingDown) return;
                    OnLedge();
                    if (!RaycastFallDown())
                    {
                    }
                }
            }
            stateMachine.ReusableData.OnLedge = false;
        }
        private void OnLedge()
        {
            stateMachine.ChangeState(stateMachine.ClimbingHighState);
        }
        protected void Velocity()
        {
            if (!stateMachine.ReusableData.IsSliding)
            {
                Vector3 playerHorizontalVelocity = stateMachine.ReusableData.CurrentJumpForce;
                Vector3 playerForward = stateMachine.Player.playerTransform.forward;

                playerHorizontalVelocity.x *= playerForward.x;
                playerHorizontalVelocity.z *= playerForward.z;

                playerHorizontalVelocity.y = stateMachine.ReusableData.VerticalVelocity;
                stateMachine.Player.CharacterController.Move(playerHorizontalVelocity * Time.deltaTime);
            }
        }
        #endregion
        #region Reusable Methods
        //protected override void OnContactWithStar(Collider collider)
        //{
        //    StopSlowmotion();
        //}
        //protected override void OnContactInStar(Collider collider)
        //{
        //    StopSlowmotion();
        //    if (slow)
        //    {
        //        //LockCamera();
        //        //mouseX += Input.GetAxisRaw("Mouse X");
        //        //collider.transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
        //        //stateMachine.Player.transform.forward = collider.transform.forward;
        //        should = true;
        //        DoSlowmotion();
        //    }
        //    else if (!slow && should)
        //    {
        //        Debug.Log("SuiSei");
        //        StopSlowmotion();
        //        //stateMachine.ChangeState(stateMachine.SuiSeiJumpState);
        //    }
        //}
        //protected override void OnContactExitWithStar(Collider collider)
        //{
        //    collider.transform.forward = Vector3.zero;
        //    UnLockCamera();
        //    StopSlowmotion();
        //    should = false;
        //}
        protected void ParticalStart()
        {
            stateMachine.Player.ParticalData.FlashBarrier.Play();
            stateMachine.Player.ParticalData.SmokeParticle.Play();
        }
        protected void ParticalStop()
        {
            stateMachine.Player.ParticalData.FlashBarrier.Stop();
            stateMachine.Player.ParticalData.SmokeParticle.Stop();
        }
        protected virtual void ResetSprintState()
        {
            stateMachine.ReusableData.ShouldSprint = false;
        }
        protected override void AddInputActionCallback()
        {
            base.AddInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Jump.started += OnJumpStrated;


        }

        protected override void RemoveInputActionCallback()
        {
            base.RemoveInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Jump.started -= OnJumpStrated;
        }
        #endregion

        #region Input Methods
        private void OnJumpStrated(InputAction.CallbackContext context)
        {
            if (!IsGround() && stateMachine.ReusableData.ShouldAirDash)
            {
                stateMachine.ChangeState(stateMachine.JumpDashState);
            }
        }
        #endregion
    }

}

