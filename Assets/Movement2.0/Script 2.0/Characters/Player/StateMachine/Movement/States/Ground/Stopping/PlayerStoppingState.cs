using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerStoppingState : PlayerGroundState
    {
        public PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.SpeedMultiplier = 0.0f;
            StartAnimation(stateMachine.Player.AnimationsData.IsStopHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationsData.IsStopHash);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            RotateTowardsTargetRotion();

            if (!IsMovingHorizontally())
            {
                return;
            }
        }
        public override void OnAnimationMove()
        {
            base.OnAnimationMove();
            DelcelerateHorizontally();
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
            stateMachine.Player.Inputs.PlayerActions.Movement.started += OnMovementStarted;
        }
        protected override void RemoveInputActionCallback()
        {
            base.RemoveInputActionCallback();
            stateMachine.Player.Inputs.PlayerActions.Movement.started -= OnMovementStarted;
        }
        #endregion
        #region Input Methods
        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }

        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }
        #endregion
    }
}


