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
            stateMachine.ReusableData.ShouldAirDash = true;
            ResetSprintState();
            StartAnimation(stateMachine.Player.AnimationsData.AirstateHash);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.ReusableData.ShouldAirDash = false;
            StopAnimation(stateMachine.Player.AnimationsData.AirstateHash);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            stateMachine.ReusableData.VerticalVelocity += stateMachine.ReusableData.Gravity * Time.deltaTime;
        }

        public override void Update()
        {
            base.Update();

            Velocity();
            AnimationFloat(stateMachine.Player.AnimationsData.VerticalVelHash, stateMachine.ReusableData.VerticalVelocity / 7.5f, 0.2f, Time.deltaTime);
        }
        #endregion
        #region Main Methods
        protected override void OnContactWithLedge(Collider collider)
        {
            stateMachine.ReusableData.OnLedge = true;
            if (stateMachine.ReusableData.OnLedge && stateMachine.ReusableData.FaceWall && !RaycastFallDown())
            {
                OnLedge();
            }
        }
        protected override void OnContactExitWithLedge(Collider collider)
        {
            stateMachine.ReusableData.OnLedge = false;
            return;
        }
        protected void OnLedge()
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

