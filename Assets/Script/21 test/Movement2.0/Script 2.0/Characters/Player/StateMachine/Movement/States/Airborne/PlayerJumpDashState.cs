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
            stateMachine.ReusableData.SuiseiJumpFinish = false;
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
            stateMachine.ReusableData.VerticalVelocity += stateMachine.ReusableData.Gravity * Time.deltaTime;
            if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero || stateMachine.ReusableData.SuiseiJumpFinish)
                Move();
        }
        public override void Update()
        {
            base.Update();
            Velocity();
            if (GetPlayerVerticalVelocity().y > -9)
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

            RotateTowardsTargetRotion();

            return directionAngle;
        }
        protected void Velocity()
        {
            if (!stateMachine.ReusableData.IsSliding)
            {
                Vector3 playerHorizontalVelocity = stateMachine.ReusableData.CurrentJumpForce;
                Vector3 playerForward = stateMachine.Player.playerTransform.forward;

                playerHorizontalVelocity.x *= playerForward.x;
                playerHorizontalVelocity.z *= playerForward.z;

                playerHorizontalVelocity.y = stateMachine.ReusableData.VerticalVelocity;
                stateMachine.Player.CharacterController.Move(playerHorizontalVelocity * Time.deltaTime);
            }
        }

        #endregion
        #region Reusable Methods

        #endregion

    }
}


