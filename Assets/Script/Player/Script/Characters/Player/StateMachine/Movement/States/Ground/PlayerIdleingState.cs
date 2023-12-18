using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerIdleingState : PlayerGroundState
    {
        public PlayerIdleingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StationaryForce;

            base.Enter();

            stateMachine.ReusableData.SpeedMultiplier = 0f;

            StartAnimation(stateMachine.Player.AnimationsData.IsIdleHash);

            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationsData.IsIdleHash);
        }
        public override void Update()
        {
            base.Update();
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero || !stateMachine.Player.CharacterController.enabled)
            {
                return;
            }
            OnMove();
            AnimationFloat(stateMachine.Player.AnimationsData.IdlestateHash, 1f, 0.1f, Time.deltaTime);
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
        #endregion

    }
}


