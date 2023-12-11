using UnityEngine.InputSystem;


namespace Nomimovment
{
    public class PlayerHardLandingState : PlayerLandingState
    {
        public PlayerHardLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
            base.Enter();
            stateMachine.Player.Inputs.PlayerActions.Movement.Disable();

            ResetVelocity();

            StartAnimation(stateMachine.Player.AnimationsData.HardLandHash);
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.Player.Inputs.PlayerActions.Movement.Enable();
            StopAnimation(stateMachine.Player.AnimationsData.HardLandHash);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!IsMovingHorizontally())
            {
                return;
            }
            ResetVelocity();
        }
        public override void OnAnimationExitEvent()
        {
            stateMachine.Player.Inputs.PlayerActions.Movement.Enable();
        }
        public override void OnAnimationTransitionEvent()
        {
            stateMachine.ChangeState(stateMachine.IdleingState);
        }
        #endregion
        #region Reusable Methods
        protected override void AddInputActionCallback()
        {
            base.AddInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Movement.started += OnMoventStarted;
        }

        protected override void RemoveInputActionCallback()
        {
            base.RemoveInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Movement.started -= OnMoventStarted;
        }
        protected override void OnMove()
        {
            if (stateMachine.ReusableData.ShouldWalk)
            {
                return;
            }
            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion
        #region Input Methods
        private void OnMoventStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}


