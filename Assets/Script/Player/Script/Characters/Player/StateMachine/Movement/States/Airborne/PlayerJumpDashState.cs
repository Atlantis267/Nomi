using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpDashState : PlayerAirborneState
{
    private bool canStartFalling;
    public PlayerJumpDashState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.SpeedMultiplier = 0.0f;

        stateMachine.ReusableData.ShouldAirDash = false;
        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.DashForce;
        stateMachine.ReusableData.IsAirDashing = true;
        Jump();
        ParticalStart();
        StartAnimation(stateMachine.Player.AnimationsData.JumpDashHash);
    }
    public override void Exit()
    {
        base.Exit();
        ParticalStop();
        canStartFalling = false;
        stateMachine.ReusableData.IsAirDashing = false;
        StopAnimation(stateMachine.Player.AnimationsData.JumpDashHash);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
    public override void Update()
    {
        base.Update();
        if (!canStartFalling && IsMovingUp(0f))
        {
            canStartFalling = true;
        }

        if (!canStartFalling || IsMovingUp(0f))
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.FallingState);
    }
    #endregion
    #region Main Methods
    private void Jump()
    {
        if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero)
        {
            Vector3 jumpDirection = GetMovementDirection();
            float targetRotationYAngle = Rotate(jumpDirection);
            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
            Vector3 jumpForce = stateMachine.ReusableData.CurrentJumpForce;
            jumpForce.x *= targetRotationDirection.x;
            jumpForce.z *= targetRotationDirection.z;
            jumpForce = GetJumpForceOnSlope(jumpForce);
            ResetVelocity();
            stateMachine.Player.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        }
        else
        {
            Vector3 jumpDirection = Vector3.zero;
            Vector3 jumpForce = stateMachine.ReusableData.CurrentJumpForce;
            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;
            jumpForce = GetJumpForceOnSlope(jumpForce);
            ResetVelocity();
            stateMachine.Player.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        }
    }
    private Vector3 GetJumpForceOnSlope(Vector3 jumpForce)
    {
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, airborneData.JumpData.JumpToGroundRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            if (IsMovingUp())
            {
                float forceModifier = airborneData.JumpData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                jumpForce.x *= forceModifier;
                jumpForce.z *= forceModifier;
            }

            if (IsMovingDown())
            {
                float forceModifier = airborneData.JumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                jumpForce.y *= forceModifier;
            }
        }

        return jumpForce;
    }
    private float Rotate(Vector3 direction)
    {
        float directionAngle = UpdateTargetRotation(direction);

        LookAt();

        return directionAngle;
    }
    private void ParticalStart()
    {
        stateMachine.Player.ParticalData.FlashBarrier.Play();
        stateMachine.Player.ParticalData.SmokeParticle.Play();
    }
    private void ParticalStop()
    {
        stateMachine.Player.ParticalData.FlashBarrier.Stop();
        stateMachine.Player.ParticalData.SmokeParticle.Stop();
    }
    #endregion
    #region Reusable Methods

    #endregion

}
