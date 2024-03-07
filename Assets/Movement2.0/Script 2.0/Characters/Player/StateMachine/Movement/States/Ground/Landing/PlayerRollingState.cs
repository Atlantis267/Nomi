using UnityEngine;
using UnityEngine.InputSystem;

namespace Movement
{
    public class PlayerRollingState : PlayerLandingState
    {
        private PlayerRollData rollData;
        public PlayerRollingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            rollData = movementData.RollData;
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            stateMachine.ReusableData.SpeedMultiplier = rollData.SpeedModifier;
            stateMachine.ReusableData.ShouldSprint = false;
            StartAnimation(stateMachine.Player.AnimationsData.RollLandHash);
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationsData.RollLandHash);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero)
            {
                return;
            }
            RotateTowardsTargetRotion();
        }
        public override void OnAnimationTransitionEvent()
        {
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.MediumStoppingState);
                return;
            }
            OnMove();
        }
        #endregion
        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
        #endregion
    }
}


