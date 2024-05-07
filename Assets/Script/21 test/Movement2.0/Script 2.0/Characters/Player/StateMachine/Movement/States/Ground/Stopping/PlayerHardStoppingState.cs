using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        public PlayerHardStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {

            base.Enter();

            stateMachine.ReusableData.MovementDelcelerationForce = movementData.StopData.HardDecelerationForce;
            StartAnimation(stateMachine.Player.AnimationsData.HardStopHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationsData.HardStopHash);
        }
        #endregion
        #region Reusable Methods
        protected override void OnMove()
        {
            if (stateMachine.ReusableData.ShouldWalk)
            {
                return;
            }
            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion
    }
}


