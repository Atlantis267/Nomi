using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerClimbingIdleingState : PlayerClimbingState
    {
        public PlayerClimbingIdleingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
            stateMachine.ReusableData.VerticalVelocity = 0.0f;
        }

        public override void Exit()
        {
            base.Exit();
            stateMachine.ReusableData.OnLedge = false;
        }
        #endregion

        #region Reusable Methods

        protected override void AddInputActionCallback()
        {
            base.AddInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Jump.started += OnJumpStart;
        }
        protected override void RemoveInputActionCallback()
        {
            base.RemoveInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Jump.started -= OnJumpStart;
        }

        #endregion

        #region Input Methods
        private void OnJumpStart(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.JumpingState);
        }
        #endregion
    }
}


