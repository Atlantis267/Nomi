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
        public override void Exit()
        {
            base.Exit();

        }
        #endregion
    }
}


