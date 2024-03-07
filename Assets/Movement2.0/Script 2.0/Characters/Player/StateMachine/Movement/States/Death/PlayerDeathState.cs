using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerDeathState : PlayerMovementState
    {
        public PlayerDeathState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();            
            StartAnimation(stateMachine.Player.AnimationsData.DiestateHash);
            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
            stateMachine.ReusableData.VerticalVelocity = 0.0f;
            stateMachine.ReusableData.IsRespawning = true;
        }
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            stateMachine.ChangeState(stateMachine.RespawnState);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationsData.DiestateHash);
        }
    }
}
