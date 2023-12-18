using UnityEngine;

namespace Movement
{
    public class PlayerLightLandingState : PlayerLandingState
    {
        public PlayerLightLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StationaryForce;
            base.Enter();
            stateMachine.ReusableData.SpeedMultiplier = 0.0f;

            ResetVelocity();
        }
        public override void Exit()
        {
            base.Exit();
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
        public override void Update()
        {
            base.Update();
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero)
            {
                return;
            }
            OnMove();
        }
        public override void OnAnimationTransitionEvent()
        {
            stateMachine.ChangeState(stateMachine.IdleingState);
        }
        #endregion
    }
}


