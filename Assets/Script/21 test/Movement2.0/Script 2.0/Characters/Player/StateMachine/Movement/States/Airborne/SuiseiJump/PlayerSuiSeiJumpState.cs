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
            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.SuiSeiForce;
            base.Enter();
            stateMachine.ReusableData.ShouldSuiseiJump = false;
            ParticalStart();
            stateMachine.ReusableData.VerticalVelocity = 10f;
            StartAnimation(stateMachine.Player.AnimationsData.SuiSeiHash);
            playerPositionOnEnter = stateMachine.Player.transform.position;
        }
        public override void Exit()
        {
            base.Exit();
            ParticalStop();
            stateMachine.ReusableData.SuiseiJumpFinish = true;
            //stateMachine.Player.CoolDownData.StartSuiSeiJumpCooldown();           
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            stateMachine.ReusableData.VerticalVelocity += stateMachine.ReusableData.Gravity * Time.deltaTime;
            Force();
        }
        public override void Update()
        {
            base.Update();
            if (GetPlayerVerticalVelocity().y > 0)
            {
                return;
            }
            stateMachine.ChangeState(stateMachine.SuiSeiFallState);
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
        #endregion
    }
}
