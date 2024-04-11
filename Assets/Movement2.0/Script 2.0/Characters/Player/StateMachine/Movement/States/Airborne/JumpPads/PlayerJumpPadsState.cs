using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerJumpPadsState : PlayerAirborneState
    {
        public PlayerJumpPadsState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            Velocity();
            stateMachine.ReusableData.ShouldAirDash = true;
            stateMachine.ReusableData.VerticalVelocity = airborneData.JumpData.JumpPadsForce;
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            stateMachine.ReusableData.VerticalVelocity += stateMachine.ReusableData.Gravity * Time.deltaTime;
            if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero)
            {
                Move();
                stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.MediumForce;
            }
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
                //playerHorizontalVelocity = GetJumpForceOnSlope(playerHorizontalVelocity);
                //ResetVelocity();

                //stateMachine.Player.Rigidbody.AddForce(playerHorizontalVelocity, ForceMode.VelocityChange);
                stateMachine.Player.CharacterController.Move(playerHorizontalVelocity * Time.deltaTime);
            }
        }
        #endregion
    }
}
