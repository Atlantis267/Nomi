using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerRespawnState : PlayerMovementState
    {
        public PlayerRespawnState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            stateMachine.Player.StartCoroutine(RespawnRoutine());
        }
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            stateMachine.ChangeState(stateMachine.IdleingState);
        }
        public override void Update()
        {
            base.Update();
            UpdateTargetRotation(stateMachine.Player.CheckData.LastPointRotate, false);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationsData.RespawnstateHash);
        }
        private IEnumerator RespawnRoutine()
        {
            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
            ResetVerticalVelocity();
            stateMachine.Player.playerTransform.position = stateMachine.Player.CheckData.LastPointPos;
            yield return null;
            StartAnimation(stateMachine.Player.AnimationsData.RespawnstateHash);
        }
    }
}
