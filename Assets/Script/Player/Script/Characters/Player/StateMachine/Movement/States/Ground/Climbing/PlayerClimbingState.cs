using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerClimbingState : PlayerMovementState
    {
        public PlayerClimbingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero)
            {
                stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeekForce;
            }
            else
            {
                stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StationaryForce;
            }
            base.Enter();
            ResetSprintState();
            StartAnimation(stateMachine.Player.AnimationsData.ClimbstateHash);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.ReusableData.OnLedge = false;
            StopAnimation(stateMachine.Player.AnimationsData.ClimbstateHash);
        }
        #endregion
        #region Reusable Methods
        protected override void AddInputActionCallback()
        {
            base.AddInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Down.started += OnDownStart;
        }

        protected override void RemoveInputActionCallback()
        {
            base.RemoveInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Down.started -= OnDownStart;
        }


        protected virtual void ResetSprintState()
        {
            stateMachine.ReusableData.ShouldSprint = false;
        }
        #endregion
        #region Input Methods

        private void OnDownStart(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.FallingingState);
        }
        #endregion
    }
}

