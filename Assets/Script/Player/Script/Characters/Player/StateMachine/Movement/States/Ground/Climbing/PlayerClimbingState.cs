using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            ResetVerticalVelocity();
            ResetSprintState();
            stateMachine.Player.Rigidbody.useGravity = false;
            StartAnimation(stateMachine.Player.AnimationsData.ClimbstateHash);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.Rigidbody.useGravity = true;
            StopAnimation(stateMachine.Player.AnimationsData.ClimbstateHash);
        }
        #endregion
        #region Reusable Methods
        protected virtual void ResetSprintState()
        {
            stateMachine.ReusableData.ShouldSprint = false;
        }
        #endregion
    }
}


