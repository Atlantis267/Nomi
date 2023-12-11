using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nomimovment
{
    public class PlayerJumpingState : PlayerAirborneState
    {
        private bool canStartFalling;

        public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
            stateMachine.ReusableData.MovementDelcelerationForce = airborneData.JumpData.DecelerationForce;
            Jump();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (IsMovingUp())
            {
                DelcelerateVertically();
            }
        }
        public override void Update()
        {
            base.Update();
            if (!canStartFalling && IsMovingUp(0f))
            {
                canStartFalling = true;
            }

            if (!canStartFalling || IsMovingUp(0f))
            {
                return;
            }

            stateMachine.ChangeState(stateMachine.FallingState);
        }
        public override void Exit()
        {
            base.Exit();
            canStartFalling = false;
            stateMachine.ReusableData.ShouldAirDash = true;
        }
        #endregion

        #region Main Methods

        private void Jump()
        {
            Vector3 jumpDirection = stateMachine.Player.transform.forward;
            Vector3 jumpForce = stateMachine.ReusableData.CurrentJumpForce;
            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;
            jumpForce = GetJumpForceOnSlope(jumpForce);
            ResetVelocity();
            stateMachine.Player.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        }
        private Vector3 GetJumpForceOnSlope(Vector3 jumpForce)
        {
            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

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
        protected override void AddInputActionCallback()
        {
            base.AddInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Jump.canceled += OnJumpCanceled;
        }

        protected override void RemoveInputActionCallback()
        {
            base.RemoveInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Jump.canceled -= OnJumpCanceled;
        }
        #endregion
        #region Input Methods
        private void OnJumpCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.FallingState);
        }
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}


