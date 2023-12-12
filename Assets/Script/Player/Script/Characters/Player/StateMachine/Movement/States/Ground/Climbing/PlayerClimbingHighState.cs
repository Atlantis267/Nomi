using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomimovment
{
    public class PlayerClimbingHighState : PlayerClimbingState
    {
        public PlayerClimbingHighState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
        }
        public override void OnAnimationMove()
        {
            base.OnAnimationMove();
            stateMachine.Player.Animator.ApplyBuiltinRootMotion();
        }
        public override void Exit()
        {
            base.Exit();

        }
        public override void OnAnimationTransitionEvent()
        {
            stateMachine.ChangeState(stateMachine.IdleingState);
        }
        #endregion
    }
}


