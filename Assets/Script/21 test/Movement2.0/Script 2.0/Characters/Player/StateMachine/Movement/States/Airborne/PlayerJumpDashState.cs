using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerJumpDashState : PlayerAirborneState
    {
        public PlayerJumpDashState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero)
            {
                stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StationaryForce;
            }
            else
            {
                stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;
            }
            base.Enter();
            stateMachine.ReusableData.VerticalVelocity = airborneData.JumpData.JumpDashForce;

            stateMachine.ReusableData.IsAirDashing = true;

            StartAnimation(stateMachine.Player.AnimationsData.JumpDashHash);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.ReusableData.IsAirDashing = false;
            stateMachine.ReusableData.ShouldAirDash = false;
            stateMachine.Player.CoolDownData.StartAirDashCooldown();
            StopAnimation(stateMachine.Player.AnimationsData.JumpDashHash);
        }
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationTransitionEvent();
            stateMachine.ChangeState(stateMachine.FallingingState);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            stateMachine.ReusableData.ShouldAirDash = false;
            if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero)
                Move();
        }
        public override void Update()
        {
            base.Update();
            Velocity();
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

        #endregion

    }
}


