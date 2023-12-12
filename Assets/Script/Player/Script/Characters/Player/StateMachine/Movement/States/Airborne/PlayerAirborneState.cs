using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nomimovment
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

            ResetSprintState();
            StartAnimation(stateMachine.Player.AnimationsData.AirstateHash);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.ReusableData.ShouldAirDash = false;
            StopAnimation(stateMachine.Player.AnimationsData.AirstateHash);
        }
        public override void Update()
        {
            base.Update();
            AnimationFloat(stateMachine.Player.AnimationsData.VerticalVelHash, stateMachine.Player.Rigidbody.velocity.y / 7.5f, 0.2f, Time.deltaTime);
        }
        #endregion
        #region Main Methods

        #endregion
        #region Reusable Methods
        protected override void OnContactWithGround(Collider collider)
        {
            stateMachine.ChangeState(stateMachine.LightLandingState);
        }
        protected override void OnContactWithLedge(Collider collider)
        {
            stateMachine.ChangeState(stateMachine.ClimbingHighState);
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
            if (!stateMachine.ReusableData.IsGrounded && stateMachine.ReusableData.ShouldAirDash)
            {
                stateMachine.ChangeState(stateMachine.JumpDashState);
            }
        }
        #endregion
    }
}


