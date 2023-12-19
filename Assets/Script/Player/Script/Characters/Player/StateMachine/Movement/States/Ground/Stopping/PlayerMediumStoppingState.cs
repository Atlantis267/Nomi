using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomimovment
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {
        public PlayerMediumStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.MediumForce;
            StartAnimation(stateMachine.Player.AnimationsData.MediumStopHash);
            stateMachine.Player.Animator.SetFloat(stateMachine.Player.AnimationsData.stoptransformHash, 1f, 0.5f, Time.deltaTime);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationsData.MediumStopHash);
        }
        #endregion
    }
}


