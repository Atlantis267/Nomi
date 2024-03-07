using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerSuiSeiFallState : PlayerAirborneState
    {
        private Vector3 playerPositionOnEnter;
        public PlayerSuiSeiFallState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region
        public override void Enter()
        {
            base.Enter();
            ResetVelocity();
            if (stateMachine.Player.CoolDownData.IsAirDashCoolingDown)
            {
                stateMachine.ReusableData.ShouldAirDash = false;
            }
            else
            {
                stateMachine.ReusableData.ShouldAirDash = true;
            }
            playerPositionOnEnter = stateMachine.Player.transform.position;
        }
        public override void Update()
        {
            base.Update();
            VelocityFalling();
            Ground();
            stateMachine.ReusableData.VerticalVelocity += stateMachine.ReusableData.Gravity * airborneData.FallData.FallMultiplier * Time.deltaTime;
        }
        public override void Exit()
        {
            base.Exit();
            StopAnimation(stateMachine.Player.AnimationsData.SuiSeiHash);
        }
        #endregion
        #region Main Methods
        private void VelocityFalling()
        {
            Vector3 playerHorizontalVelocity = stateMachine.ReusableData.CurrentJumpForce;
            UpdateTargetRotation(stateMachine.ReusableData.SuiSeiJumpDir, false);
            playerHorizontalVelocity.x *= stateMachine.Player.playerTransform.forward.x;
            playerHorizontalVelocity.z *= stateMachine.Player.playerTransform.forward.z;
            playerHorizontalVelocity.y = stateMachine.ReusableData.VerticalVelocity;
            stateMachine.Player.CharacterController.Move(playerHorizontalVelocity * Time.deltaTime);
        }
        private void Ground()
        {
            if (/*Physics.SphereCast(stateMachine.Player.playerTransform.position + (Vector3.up * stateMachine.ReusableData.GroundCheckOffset)
                , stateMachine.Player.CharacterController.radius, Vector3.down, out RaycastHit hit
                , stateMachine.ReusableData.GroundCheckOffset - stateMachine.Player.CharacterController.radius + 2 * stateMachine.Player.CharacterController.skinWidth
                , stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore)*/IsGround())
            {
                stateMachine.ChangeState(stateMachine.RollingState);
            }
            else
            {
                return;
            }
        }
        #endregion
    }
}
