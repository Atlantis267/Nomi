using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomimovment
{
    public class PlayerIdleingState : PlayerGroundState
    {
        private PlayerIdleData idleData;
        public PlayerIdleingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            idleData = movementData.IdleData;
        }
        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.SpeedMultiplier = 0f;

            stateMachine.ReusableData.BackwardsCameraRecenteringData = idleData.BackwardsCameraRecenteringData;

            base.Enter();

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StationaryForce;

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
            if (stateMachine.ReusableData.CurrentMovementInput == Vector2.zero)
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


