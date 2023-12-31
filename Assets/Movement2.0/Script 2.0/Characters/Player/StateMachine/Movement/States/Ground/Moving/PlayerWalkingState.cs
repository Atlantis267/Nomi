using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerWalkingState : PlayerMovingState
    {
        public int pos;
        public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeekForce;

            base.Enter();

            stateMachine.ReusableData.SpeedMultiplier = movementData.WalkData.SpeedModifier;

        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void Update()
        {
            base.Update();

            AnimationFloat(stateMachine.Player.AnimationsData.MoveSpeedHash, stateMachine.ReusableData.CurrentMovementInput.magnitude * stateMachine.ReusableData.SpeedMultiplier, 0.1f, Time.deltaTime);
        }
        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();
            pos = (int)Mathf.Floor(UnityEngine.Random.Range(0, stateMachine.Player.SoundData.WalkSound.Count));
            stateMachine.Player.SoundData.audioSource.PlayOneShot(stateMachine.Player.SoundData.WalkSound[pos]);
        }
        #endregion


        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.LightStoppingState);
        }
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion
    }
}


