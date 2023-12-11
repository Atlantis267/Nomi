using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Nomimovment
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private PlayerSprintData sprintData;
        private float startTime;
        private bool shouldResetSprintState;

        public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            sprintData = movementData.SprintData;
        }
        #region IState Methods
        public override void Enter()
        {

            stateMachine.ReusableData.SpeedMultiplier = sprintData.SpeedModifier;

            base.Enter();

            stateMachine.ReusableData.IsSprinting = true;
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

            startTime = Time.time;
            shouldResetSprintState = true;
            if (!stateMachine.ReusableData.ShouldSprint)
            {
                stateMachine.ReusableData.KeepSprint = false;
            }

        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.ReusableData.IsSprinting = false;

            if (shouldResetSprintState)
            {
                stateMachine.ReusableData.KeepSprint = false;

                stateMachine.ReusableData.ShouldSprint = false;
            }
        }
        public override void Update()
        {
            base.Update();
            if (stateMachine.ReusableData.KeepSprint)
            {
                return;
            }
            if (Time.time < startTime + sprintData.SprintToRunTime)
            {
                return;
            }

            AnimationFloat(stateMachine.Player.AnimationsData.MoveSpeedHash, stateMachine.ReusableData.CurrentMovementInput.magnitude * stateMachine.ReusableData.SpeedMultiplier, 0.1f, Time.deltaTime);
            StopSprinting();
        }

        #endregion
        #region Main Methods

        private void StopSprinting()
        {
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.HardStoppingState);

                return;
            }
            stateMachine.ChangeState(stateMachine.RunningState);
        }

        #endregion
        #region Reusable Methods
        protected override void AddInputActionCallback()
        {
            base.AddInputActionCallback();

            stateMachine.Player.Inputs.PlayerActions.Sprint.performed += OnSprintPerformed;
        }
        protected override void RemoveInputActionCallback()
        {
            base.RemoveInputActionCallback();

            stateMachine.Player.Inputs.PlayerActions.Sprint.performed -= OnSprintPerformed;
        }
        protected override void OnFalling()
        {
            shouldResetSprintState = false;
            base.OnFalling();
        }
        #endregion
        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.HardStoppingState);
            base.OnMovementCanceled(context);
        }
        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            stateMachine.ReusableData.KeepSprint = true;
            stateMachine.ReusableData.ShouldSprint = true;
        }

        #endregion
    }
}

