using UnityEngine;

namespace Movement
{
    public class PlayerMovingState : PlayerGroundState
    {
        public PlayerMovingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        public override void Enter()
        {
            base.Enter();
            if (stateMachine.ReusableData.IsSliding)
            {
                return;
            }
            StartAnimation(stateMachine.Player.AnimationsData.IsMoveHash);
        }
        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationsData.IsMoveHash);
        }
    }
}

