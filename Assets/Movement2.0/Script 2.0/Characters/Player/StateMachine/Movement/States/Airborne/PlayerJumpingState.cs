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
            stateMachine.ReusableData.ShouldAirDash = true;
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

        private Vector3 GetJumpForceOnSlope(Vector3 jumpForce)
        {
            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtilitiy.CapsuleColliderData.characterController.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, airborneData.JumpData.JumpToGroundRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                if (IsMovingUp())
                {
                    float forceModifier = airborneData.JumpData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;
                }

                if (IsMovingDown())
                {
                    float forceModifier = airborneData.JumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                    jumpForce.y *= forceModifier;
                }
            }

            return jumpForce;
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

