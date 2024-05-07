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
            stateMachine.ReusableData.SuiseiJumpFinish = false;
            StopAnimation(stateMachine.Player.AnimationsData.AirstateHash);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            IsOnLedge();
        }
        public override void Update()
        {
            base.Update();
            Bash();
            AnimationFloat(stateMachine.Player.AnimationsData.VerticalVelHash, stateMachine.ReusableData.VerticalVelocity / 7.5f, 0.2f, Time.deltaTime);
        }
        #endregion

        #region Main Methods
        protected void IsOnLedge()
        {
            Vector3 DownStart = (stateMachine.Player.transform.position + Vector3.up * 2.2f) + stateMachine.Player.transform.forward * 0.5f;
            Vector3 DownEnd = (stateMachine.Player.transform.position + Vector3.up * 0.7f) + stateMachine.Player.transform.forward * 0.5f;
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
        #endregion
        #region Reusable Methods
        protected void ParticalStart()
        {
            stateMachine.Player.ParticalData.FlashBarrier.Play();
            stateMachine.Player.ParticalData.SmokeParticle.Play();
            stateMachine.Player.ParticalData.HeatParticle.Play();
        }
        protected void ParticalStop()
        {
            stateMachine.Player.ParticalData.FlashBarrier.Stop();
            stateMachine.Player.ParticalData.SmokeParticle.Stop();
            stateMachine.Player.ParticalData.HeatParticle.Stop();
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

