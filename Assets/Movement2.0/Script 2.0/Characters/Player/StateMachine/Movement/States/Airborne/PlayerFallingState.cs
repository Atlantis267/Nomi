using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerFallingState : PlayerAirborneState
    {
        private Vector3 playerPositionOnEnter;
        public PlayerFallingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }
        #region IState Methods
        public override void Enter()
        {
            base.Enter();
            stateMachine.ReusableData.IsFalling = true;
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
            stateMachine.ReusableData.VerticalVelocity += stateMachine.ReusableData.Gravity * airborneData.FallData.FallMultiplier * Time.deltaTime;
            Ground();
        }
        public override void Exit()
        {
            base.Exit();
            stateMachine.ReusableData.IsFalling = false;
        }
        #endregion
        #region Main Methods
        private void VelocityFalling()
        {
            Vector3 playerHorizontalVelocity = stateMachine.ReusableData.CurrentJumpForce;

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
                //Debug.Log("isGround");
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
            else
            {
                return;
            }
        }
        #endregion
    }
}


