using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerMovementState
{
    private Vector3 slopSlideVelocity;
    public PlayerGroundState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();

        UpdateShouldSprintState();
        UpdateCameraRecenteringState(stateMachine.ReusableData.CurrentMovementInput);

        StartAnimation(stateMachine.Player.AnimationsData.GroundstateHash);

    }
    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationsData.GroundstateHash);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Float();
        OnSliding();
    }
    public override void Update()
    {
        base.Update();

        SetSlopeSlideVelocity();

        AnimationFloat(stateMachine.Player.AnimationsData.MoveSpeedHash, stateMachine.ReusableData.CurrentMovementInput.magnitude * stateMachine.ReusableData.SpeedMultiplier, 0.1f, Time.deltaTime);
        AnimationFloat(stateMachine.Player.AnimationsData.stoptransformHash, 0.0f, 0.2f, Time.deltaTime);
    }
    #region Main Methods
    private void Float()
    {

        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtilitiy.CapsuleColliderData.Collider.bounds.center;
        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);
        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, stateMachine.Player.ColliderUtilitiy.SlopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (slopeSpeedModifier == 0f)
            {
                return;
            }

            float distanceToFloatingPoint = stateMachine.Player.ColliderUtilitiy.CapsuleColliderData.ColliderCenterInLocalSpace.y * stateMachine.Player.transform.localScale.y - hit.distance;
            if (distanceToFloatingPoint == 0f)
            {
                return;
            }
            float amountToLift = distanceToFloatingPoint * stateMachine.Player.ColliderUtilitiy.SlopeData.StepReachForce - GetPlayerVerticalVelocity().y;
            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

            stateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }
    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = movementData.SlopeSpeedAngles.Evaluate(angle);

        if(stateMachine.ReusableData.SlopeSpeedMultiplier != slopeSpeedModifier)
        {
            stateMachine.ReusableData.SlopeSpeedMultiplier = slopeSpeedModifier;
            UpdateCameraRecenteringState(stateMachine.ReusableData.CurrentMovementInput);
        }

        return slopeSpeedModifier;
    }
    protected override void OnContactWithGroundExit(Collider collider)
    {
        if (IsThereGroundUnderneath())
        {
            Debug.Log("return");
            return;
        }
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtilitiy.CapsuleColliderData.Collider.bounds.center;
        Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - stateMachine.Player.ColliderUtilitiy.CapsuleColliderData.ColliderVerticalEvtents, Vector3.down);
        if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, movementData.GroundFallRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            OnFalling();
        }
    }
    private bool IsThereGroundUnderneath()
    {
        PlayerTriggerColliderData triggerColliderData = stateMachine.Player.ColliderUtilitiy.TriggerColliderData;

        Vector3 groundColliderCenterInWorldSpace = triggerColliderData.GroundCheckCollider.bounds.center;

        Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, triggerColliderData.GroundCheckColliderVerticalExtents, triggerColliderData.GroundCheckCollider.transform.rotation, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore);

        return overlappedGroundColliders.Length > 0;
    }
    protected virtual void OnFalling()
    {
        stateMachine.ChangeState(stateMachine.FallingState);
    }
    private void OnSliding()
    {
        if (slopSlideVelocity == Vector3.zero)
        {
            stateMachine.ReusableData.IsSliding = false;
        }
        if (slopSlideVelocity != Vector3.zero)
        {
            Debug.Log("sliding");
            stateMachine.ReusableData.IsSliding = true;
        }
        if (stateMachine.ReusableData.IsSliding)
        {
            //ResetVelocity();

        }
    }
    private void SetSlopeSlideVelocity()
    {
        if (Physics.Raycast(stateMachine.Player.playerTransform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);

            if (angle >= stateMachine.Player.ColliderUtilitiy.CapsuleColliderData.slopeLimit)
            {
                slopSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, stateMachine.Player.Rigidbody.velocity.y, 0), hit.normal);
                return;
            }
        }
        //if (stateMachine.ReusableData.IsSliding)
        //{
        //    //slopSlideVelocity -= slopSlideVelocity * Time.deltaTime *3;
        //    if (slopSlideVelocity.magnitude > 1)
        //    {
        //        return;
        //    }
        //}

        slopSlideVelocity = Vector3.zero;
    }
    private void UpdateShouldSprintState()
    {
        if (!stateMachine.ReusableData.ShouldSprint)
        {
            return;
        }

        if (stateMachine.ReusableData.CurrentMovementInput != Vector2.zero)
        {
            return;
        }

        stateMachine.ReusableData.ShouldSprint = false;
    }


    #endregion
    #region Reusable Methods
    protected override void AddInputActionCallback()
    {
        base.AddInputActionCallback();
        stateMachine.Player.Inputs.PlayerActions.Dash.started += OnDashStarted;
        stateMachine.Player.Inputs.PlayerActions.Jump.started += OnJumpStarted;
    }

    protected override void RemoveInputActionCallback()
    {
        base.RemoveInputActionCallback();
        stateMachine.Player.Inputs.PlayerActions.Dash.started -= OnDashStarted;
        stateMachine.Player.Inputs.PlayerActions.Jump.started -= OnJumpStarted;
    }
    protected virtual void OnMove()
    {
        if (stateMachine.ReusableData.ShouldSprint)
        {
            stateMachine.ChangeState(stateMachine.SprintingState);

            return;
        }
        if (stateMachine.ReusableData.ShouldWalk)
        {
            stateMachine.ChangeState(stateMachine.WalkingState);

            return;
        }
        stateMachine.ChangeState(stateMachine.RunningState);
    }
    #endregion
    #region Input Methods
    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    {
        if(stateMachine.ReusableData.CurrentMovementInput != Vector2.zero && !stateMachine.ReusableData.IsSliding && stateMachine.ReusableData.ShouldDash)
        {
            stateMachine.ChangeState(stateMachine.DashingState);
        }
    }
    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        if (stateMachine.ReusableData.IsGrounded && !stateMachine.ReusableData.IsSliding)
        {
            stateMachine.ChangeState(stateMachine.JumpingState);
        }
    }
    #endregion
}
