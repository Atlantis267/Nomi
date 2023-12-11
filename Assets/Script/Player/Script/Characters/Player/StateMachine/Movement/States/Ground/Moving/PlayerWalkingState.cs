using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nomimovment
{
    public class PlayerWalkingState : PlayerMovingState
    {
        private PlayerWalkData walkData;
        public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            walkData = movementData.WalkData;
        }
        #region IState Methods
        public override void Enter()
        {

            stateMachine.ReusableData.SpeedMultiplier = walkData.SpeedModifier;

            stateMachine.ReusableData.BackwardsCameraRecenteringData = walkData.BackwardsCameraRecenteringData;

            base.Enter();

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;

        }
        public override void Exit()
        {
            base.Exit();
            SetBaseCameraRecenteringData();
        }
        public override void Update()
        {
            base.Update();

            AnimationFloat(stateMachine.Player.AnimationsData.MoveSpeedHash, stateMachine.ReusableData.CurrentMovementInput.magnitude * stateMachine.ReusableData.SpeedMultiplier, 0.1f, Time.deltaTime);
        }
        #endregion


        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.LightStoppingState);

            base.OnMovementCanceled(context);
        }
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion
    }
}


