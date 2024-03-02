using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerSuiSeiJumpState : PlayerAirborneState
    {
        private Vector3 playerPositionOnEnter;
        public PlayerSuiSeiJumpState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;
            base.Enter();
            stateMachine.ReusableData.ShouldSuiseiJump = false;
            ParticalStart();
            stateMachine.ReusableData.VerticalVelocity = 5f;
            StartAnimation(stateMachine.Player.AnimationsData.SuiSeiHash);
            playerPositionOnEnter = stateMachine.Player.transform.position;
        }
        public override void Exit()
        {
            base.Exit();
            ParticalStop();
            //stateMachine.Player.CoolDownData.StartSuiSeiJumpCooldown();
            StopAnimation(stateMachine.Player.AnimationsData.SuiSeiHash);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Force();
        }
        public override void Update()
        {
            base.Update();
            Ground();
            Velocity();
            if (GetPlayerVerticalVelocity().y > 0)
            {
                return;
            }
            stateMachine.ChangeState(stateMachine.FallingingState);
        }

        #endregion
        #region Main Methods
        private void Force()
        {
            if (!stateMachine.ReusableData.IsSliding)
            {
                Vector3 playerHorizontalVelocity = stateMachine.ReusableData.CurrentJumpForce;
                stateMachine.Player.transform.forward = stateMachine.ReusableData.SuiSeiJumpDir;
                Vector3 playerForward = stateMachine.Player.transform.forward;

                playerHorizontalVelocity.x *= playerForward.x;
                playerHorizontalVelocity.z *= playerForward.z;

                playerHorizontalVelocity.y = stateMachine.ReusableData.VerticalVelocity;
                stateMachine.Player.CharacterController.Move(playerHorizontalVelocity * Time.deltaTime);
            }
        }
        private void Ground()
        {
            if (IsGround())
            {
                float fallDistance = playerPositionOnEnter.y - stateMachine.Player.transform.position.y;
                if (fallDistance < airborneData.FallData.MinimumDistanceHardFall)
                {
                    stateMachine.ChangeState(stateMachine.LightLandingState);

                    return;
                }
                if (stateMachine.ReusableData.ShouldWalk && !stateMachine.ReusableData.ShouldSprint || stateMachine.ReusableData.CurrentMovementInput == Vector2.zero)
                {
                    stateMachine.ChangeState(stateMachine.HardLandingState);

                    return;
                }

                stateMachine.ChangeState(stateMachine.RollingState);
            }
        }
        #endregion
    }
}
