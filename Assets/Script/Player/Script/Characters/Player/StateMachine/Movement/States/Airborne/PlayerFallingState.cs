using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        stateMachine.ReusableData.SpeedMultiplier = 0.0f;

        ResetVerticalVelocity();

        playerPositionOnEnter = stateMachine.Player.transform.position;
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        LimitVerticalVelocity();
        ContactWithGroundCheck();
    }
    public override void Exit()
    {
        base.Exit();
    }
    #endregion
    #region Main Methods
    private void LimitVerticalVelocity()
    {
        Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

        if (playerVerticalVelocity.y >= -airborneData.FallData.FallSpeedLimit)
        {
            return;
        }

        Vector3 limitedVelocityForce = new Vector3(0f, -airborneData.FallData.FallSpeedLimit - playerVerticalVelocity.y, 0f);

        stateMachine.Player.Rigidbody.AddForce(limitedVelocityForce, ForceMode.VelocityChange);
    }
    private void ContactWithGroundCheck()
    {
        if (CheckGround())
        {
            //Debug.Log("isGround");
            float fallDistance = Mathf.Abs(playerPositionOnEnter.y - stateMachine.Player.transform.position.y);
            if (fallDistance < airborneData.FallData.MinimumDistanceToBeConsideredHardFall)
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
    #region Reusable Methods
    protected override void OnContactWithGround(Collider collider)
    {
        float fallDistance = playerPositionOnEnter.y - stateMachine.Player.transform.position.y;
        if (fallDistance < airborneData.FallData.MinimumDistanceToBeConsideredHardFall)
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
    protected override void ResetSprintState()
    {
    }
    #endregion
}
