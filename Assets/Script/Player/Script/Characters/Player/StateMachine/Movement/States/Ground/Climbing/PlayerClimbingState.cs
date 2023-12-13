using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nomimovment
{
    public class PlayerClimbingState : PlayerMovementState
    {
        public PlayerClimbingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            ResetVelocity();
            ResetSprintState();
            stateMachine.Player.Rigidbody.useGravity = false;
            StartAnimation(stateMachine.Player.AnimationsData.ClimbstateHash);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.Rigidbody.useGravity = true;
            stateMachine.ReusableData.OnLedge = false;
            StopAnimation(stateMachine.Player.AnimationsData.ClimbstateHash);
        }
        #endregion
        #region Reusable Methods
        protected override void AddInputActionCallback()
        {
            base.AddInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.TestButton.started += TestingStart;
        }
        protected override void RemoveInputActionCallback()
        {
            base.RemoveInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.TestButton.started -= TestingStart;
        }
        protected virtual void ResetSprintState()
        {
            stateMachine.ReusableData.ShouldSprint = false;
        }
        #endregion
        #region Input Methods
        protected virtual void TestingStart(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.IdleingState);
        }
        #endregion
    }
}


