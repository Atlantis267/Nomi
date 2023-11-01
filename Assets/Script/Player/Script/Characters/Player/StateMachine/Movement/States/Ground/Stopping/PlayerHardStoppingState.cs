using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHardStoppingState : PlayerStoppingState
{
    public PlayerHardStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods
    public override void Enter()
    {
        base.Enter();

        stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;
        StartAnimation(stateMachine.Player.AnimationsData.HardStopHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationsData.HardStopHash);
    }
    #endregion
    #region Reusable Methods
    protected override void OnMove()
    {
        if (stateMachine.ReusableData.ShouldWalk)
        {
            return;
        }
        stateMachine.ChangeState(stateMachine.RunningState);
    }
    #endregion
}
