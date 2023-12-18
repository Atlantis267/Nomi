using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerClimbingUpState : PlayerAirborneState
    {
        public PlayerClimbingUpState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
            stateMachine.ReusableData.VerticalVelocity = 10f;
            StartAnimation(stateMachine.Player.AnimationsData.ClimbUpstateHash);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.ReusableData.OnLedge = false;
            StopAnimation(stateMachine.Player.AnimationsData.ClimbUpstateHash);
        }
        public override void OnAnimationTransitionEvent()
        {
        }
        #endregion
    }
}


