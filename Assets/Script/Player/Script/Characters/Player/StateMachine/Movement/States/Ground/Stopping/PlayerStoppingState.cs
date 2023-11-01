using UnityEngine.InputSystem;

public class PlayerStoppingState : PlayerGroundState
{
    public PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {

        stateMachine.ReusableData.SpeedMultiplier = 0.0f;

        SetBaseCameraRecenteringData();

        base.Enter();

        StartAnimation(stateMachine.Player.AnimationsData.IsStopHash);
        ResetVelocity();
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationsData.IsStopHash);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        LookAt();
        if (!IsMovingHorizontally())
        {
            return;
        }
        DelcelerateHorizontally();
    }
    public override void OnAnimationTransitionEvent()
    {
        stateMachine.ChangeState(stateMachine.IdleingState);
    }
    #endregion
    #region Reusable Methods
    protected override void AddInputActionCallback()
    {
        base.AddInputActionCallback();
        stateMachine.Player.Inputs.PlayerActions.Movement.started += OnMovementStarted;
    }
    protected override void RemoveInputActionCallback()
    {
        base.RemoveInputActionCallback();
        stateMachine.Player.Inputs.PlayerActions.Movement.started -= OnMovementStarted;
    }
    #endregion
    #region Input Methods

    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }
    #endregion
}
