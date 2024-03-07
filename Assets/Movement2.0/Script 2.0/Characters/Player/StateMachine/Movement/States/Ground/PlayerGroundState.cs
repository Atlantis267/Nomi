using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerGroundState : PlayerMovementState
    {
        private Vector3 slopSlideVelocity;

        public PlayerGroundState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            UpdateShouldSprintState();

            StartAnimation(stateMachine.Player.AnimationsData.GroundstateHash);
        }
        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationsData.GroundstateHash);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            SetSlopeSlideVelocity();
            Gravity();
            OnFalling();
        }
        public override void Update()
        {
            base.Update();


            OnSliding();

            AnimationFloat(stateMachine.Player.AnimationsData.MoveSpeedHash, stateMachine.ReusableData.CurrentMovementInput.magnitude * stateMachine.ReusableData.SpeedMultiplier, 0.1f, Time.deltaTime);
            AnimationFloat(stateMachine.Player.AnimationsData.stoptransformHash, 0.0f, 0.2f, Time.deltaTime);
        }
        #region Main Methods

        private void Gravity()
        {
            if (!stateMachine.ReusableData.IsSliding)
            {
                if (IsGround())
                {
                    float amountToLift = stateMachine.ReusableData.GroundedGravity;
                    stateMachine.ReusableData.VerticalVelocity = amountToLift;
                    Vector3 liftForce = new Vector3(0f, amountToLift, 0f);
                    stateMachine.Player.CharacterController.Move(liftForce * Time.deltaTime);
                }
            }
        }
        private void OnFalling()
        {
            if (!IsGround())
            {
                Vector3 playerCenterInWorldSpace = stateMachine.Player.CharacterController.bounds.center;
                Ray downwardRay = new Ray(playerCenterInWorldSpace - stateMachine.Player.ColliderUtilitiy.CapsuleColliderData.ColliderVerticalEvtents, Vector3.down);
                if (!Physics.Raycast(downwardRay, out _, movementData.GroundFallRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
                {
                    stateMachine.ChangeState(stateMachine.FallingingState);
                }
            }
        }
        private void OnSliding()
        {
            if (slopSlideVelocity == Vector3.zero)
            {
                stateMachine.ReusableData.IsSliding = false;
            }
            if (slopSlideVelocity != Vector3.zero)
            {
                stateMachine.ReusableData.IsSliding = true;
            }
            if (OnSteepSlope())
            {
                Vector3 velocity = slopSlideVelocity;

                stateMachine.ReusableData.CurrentMovementInput = Vector2.zero;

                velocity.y = stateMachine.ReusableData.SlopeGravity;

                stateMachine.Player.CharacterController.Move(velocity * Time.deltaTime);
            }
        }
        private void UpdateShouldSprintState()
        {
            if (!stateMachine.ReusableData.ShouldSprint)
            {
                return;
            }

            if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero)
            {
                return;
            }

            stateMachine.ReusableData.ShouldSprint = false;
        }

        protected bool OnSteepSlope()
        {
            if (Physics.Raycast(stateMachine.Player.playerTransform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);

                if (angle > stateMachine.Player.CharacterController.slopeLimit) return true;
            }
            return false;
        }
        private void SetSlopeSlideVelocity()
        {
            if (Physics.Raycast(stateMachine.Player.playerTransform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);

                if (angle > stateMachine.Player.CharacterController.slopeLimit)
                {
                    slopSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, stateMachine.ReusableData.SlopeGravity, 0), hit.normal);
                    return;
                }
            }
            if (OnSteepSlope())
            {
                slopSlideVelocity -= slopSlideVelocity * Time.deltaTime * 3;
                if (slopSlideVelocity.magnitude > 1)
                {
                    return;
                }
            }

            slopSlideVelocity = Vector3.zero;
        }

        #endregion
        #region Reusable Methods
        protected override void AddInputActionCallback()
        {
            base.AddInputActionCallback();

            stateMachine.Player.Inputs.PlayerActions.Movement.canceled += OnMovementCanceled;
            stateMachine.Player.Inputs.PlayerActions.Dash.started += OnDashStarted;
            stateMachine.Player.Inputs.PlayerActions.Jump.started += OnJumpStarted;
        }

        protected override void RemoveInputActionCallback()
        {
            base.RemoveInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Movement.canceled -= OnMovementCanceled;
            stateMachine.Player.Inputs.PlayerActions.Dash.started -= OnDashStarted;
            stateMachine.Player.Inputs.PlayerActions.Jump.started -= OnJumpStarted;
        }
        protected virtual void OnMove()
        {
            if (stateMachine.ReusableData.ShouldSprint)
            {
                stateMachine.ChangeState(stateMachine.SprintingState);

                return;
            }
            if (stateMachine.ReusableData.ShouldWalk)
            {
                stateMachine.ChangeState(stateMachine.WalkingState);

                return;
            }
            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion
        #region Input Methods
        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
            if (IsGround())
            {
                stateMachine.ChangeState(stateMachine.IdleingState);
            }
        }
        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero && !stateMachine.ReusableData.IsSliding && stateMachine.ReusableData.ShouldDash)
            {
                stateMachine.ChangeState(stateMachine.DashingState);
            }
        }
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            if (IsGround() && !stateMachine.ReusableData.IsSliding)
            {
                stateMachine.ChangeState(stateMachine.JumpingState);
            }
        }
        #endregion
    }

}

