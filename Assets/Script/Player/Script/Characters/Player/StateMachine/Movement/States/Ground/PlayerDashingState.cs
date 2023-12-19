using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nomimovment
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
            stateMachine.ReusableData.SpeedMultiplier = dashData.SpeedModifier;

            base.Enter();

            stateMachine.ReusableData.IsDashing = true;
            stateMachine.ReusableData.RotatonData = dashData.RotationData;
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;
            shouldKeepRotating = stateMachine.ReusableData.CurrentMovementInput != Vector2.zero;

            Dash();
            UpdateConsecutiveDashes();
            FeetTween();

            startTime = Time.time;

            ParticalStart();

            StartAnimation(stateMachine.Player.AnimationsData.IsDashHash);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.ReusableData.IsDashing = false;

            SetBaseRotationData();

            ParticalStop();

            StopAnimation(stateMachine.Player.AnimationsData.IsDashHash);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!shouldKeepRotating)
            {
                return;
            }
            LookAt();
        }
        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();

            stateMachine.Player.Animator.SetFloat(stateMachine.Player.AnimationsData.FeetTweenHash, feetTween);
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
        private void Dash()
        {
            Vector3 dashDirection = stateMachine.Player.transform.forward;

            dashDirection.y = 0f;
            UpdateTargetRotation(dashDirection, false);
            if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero)
            {
                UpdateTargetRotation(GetMovementDirection());

                dashDirection = GetTargetRotationDirection(stateMachine.ReusableData.currenttargetRotation);
            }
            stateMachine.Player.Rigidbody.velocity = dashDirection * GetMovementSpeed(false);
            //stateMachine.Player.CharacterController.Move(dashDirection * GetMovementSpeed());
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
            stateMachine.Player.ParticalData.SmokeParticle.Play();
        }

        private void ParticalStop()
        {
            stateMachine.Player.ParticalData.FlashBarrier.Stop();
            stateMachine.Player.ParticalData.SmokeParticle.Stop();
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


