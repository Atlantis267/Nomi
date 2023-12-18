using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerJumpingState : PlayerAirborneState
    {
        public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            Velocity();
            stateMachine.ReusableData.VerticalVelocity = airborneData.JumpData.JumpForce;
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            stateMachine.ReusableData.VerticalVelocity += stateMachine.ReusableData.Gravity * Time.deltaTime;
            if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero)
                Move();
        }
        public override void Update()
        {
            base.Update();
            if (GetPlayerVerticalVelocity().y > 0)
            {
                return;
            }
            stateMachine.ChangeState(stateMachine.FallingingState);
        }
        #endregion

        #region Main Methods
        private void Move()
        {
            Vector3 movementDirection = GetMovementDirection();
            Rotate(movementDirection);
            movementDirection.Normalize();
        }
        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);

            LookAt();

            return directionAngle;
        }
        #endregion

        #region Reusable Methods
        //protected override void AddInputActionCallback()
        //{
        //    base.AddInputActionCallback();
        //    stateMachine.Player.Inputs.PlayerActions.Jump.canceled += OnJumpCanceled;
        //}

        //protected override void RemoveInputActionCallback()
        //{
        //    base.RemoveInputActionCallback();
        //    stateMachine.Player.Inputs.PlayerActions.Jump.canceled -= OnJumpCanceled;
        //}
        #endregion
        #region Input Methods
        //private void OnJumpCanceled(InputAction.CallbackContext context)
        //{
        //    stateMachine.ChangeState(stateMachine.FallingingState);
        //}
        #endregion
    }

}

