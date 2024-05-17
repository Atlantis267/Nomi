using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerDashingState : PlayerGroundState
    {
        private PlayerDashData dashData;

        private float startTime;
        private int consecutiveDashesUsed;

        private bool shouldKeepRotating;
        private float feetTween;
        public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            dashData = movementData.DashData;
        }
        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

            base.Enter();
            stateMachine.ReusableData.IsDashing = true;

            stateMachine.ReusableData.SpeedMultiplier = dashData.SpeedModifier;
            stateMachine.ReusableData.RotatonData = dashData.RotationData;
            shouldKeepRotating = stateMachine.ReusableData.CurrentMovementInput != Vector2.zero;

            AddForceState();
            UpdateConsecutiveDashes();
            FeetTween();
            //Showbady(false);

            startTime = Time.time;

            StartAnimation(stateMachine.Player.AnimationsData.IsDashHash);
            ParticalStart();
            stateMachine.Player.ParticalData.CameraParticle.Play();
            stateMachine.Player.Animator.SetFloat(stateMachine.Player.AnimationsData.FeetTweenHash, feetTween);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.ReusableData.IsDashing = false;

            SetBaseRotationData();
            ParticalStop();
            //Showbady(true);

            StopAnimation(stateMachine.Player.AnimationsData.IsDashHash);

        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!shouldKeepRotating)
            {
                return;
            }
            RotateTowardsTargetRotion();
        }
        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();
        }
        public override void OnAnimationTransitionEvent()
        {
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.HardStoppingState);
                return;
            }
            stateMachine.ChangeState(stateMachine.SprintingState);
        }
        #endregion
        #region Main Methods
        private void AddForceState()
        {
            Vector3 dashDirection = stateMachine.Player.transform.forward;

            dashDirection.y = 0f;
            UpdateTargetRotation(dashDirection, false);
            if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero)
            {
                UpdateTargetRotation(GetMovementDirection());

                dashDirection = GetTargetRotationDirection(stateMachine.ReusableData.currenttargetRotation);
            }

            stateMachine.Player.CharacterController.Move(dashDirection * GetMovementSpeed());
        }

        private void UpdateConsecutiveDashes()
        {
            if (!IsConsecutive())
            {
                consecutiveDashesUsed = 0;
            }

            ++consecutiveDashesUsed;

            if (consecutiveDashesUsed == dashData.ConsecutiveDashesLimitAmount)
            {
                consecutiveDashesUsed = 0;

                stateMachine.Player.Inputs.DisableActionFor(stateMachine.Player.Inputs.PlayerActions.Dash, dashData.DashLimitReachedCooldown);
            }
        }
        private void FeetTween()
        {
            feetTween = Mathf.Repeat(stateMachine.Player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime, 1);
            feetTween = feetTween < 0.5f ? 1 : -1;
            feetTween *= 3;
            feetTween = UnityEngine.Random.Range(0.5f, 1f) * feetTween;
        }

        private bool IsConsecutive()
        {
            return Time.time < startTime + dashData.TimeToBeConsideredConsecutive;
        }
        private void ParticalStart()
        {
            stateMachine.Player.ParticalData.FlashBarrier.Play();
            ShakeCamera();
            stateMachine.Player.ParticalData.SmokeParticle.Play();
            stateMachine.Player.ParticalData.HeatParticle.Play();
        }

        private void ParticalStop()
        {
            stateMachine.Player.ParticalData.FlashBarrier.Stop();
            stateMachine.Player.ParticalData.SmokeParticle.Stop();
            stateMachine.Player.ParticalData.HeatParticle.Stop();
        }
        #endregion
        #region Reusable Methods
        protected override void AddInputActionCallback()
        {
            base.AddInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Movement.performed += OnMoventPerformed;
        }

        protected override void RemoveInputActionCallback()
        {
            base.RemoveInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Movement.performed -= OnMoventPerformed;
        }

        #endregion
        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.HardStoppingState);
            stateMachine.Player.ParticalData.CameraParticle.Stop();
        }

        private void OnMoventPerformed(InputAction.CallbackContext context)
        {
            shouldKeepRotating = true;
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}


