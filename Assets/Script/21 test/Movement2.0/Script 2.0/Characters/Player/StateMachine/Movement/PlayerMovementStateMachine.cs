using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class PlayerMovementStateMachine : StateMachine
    {
        public Player Player { get; }
        public PlayerStateReusableData ReusableData { get; }
        public PlayerRespawnState RespawnState { get; }
        public PlayerDeathState DeathState { get; }
        public PlayerIdleingState IdleingState { get; }
        public PlayerDashingState DashingState { get; }
        public PlayerWalkingState WalkingState { get; }
        public PlayerRunningState RunningState { get; }
        public PlayerSprintingState SprintingState { get; }
        public PlayerLightStoppingState LightStoppingState { get; }
        public PlayerMediumStoppingState MediumStoppingState { get; }
        public PlayerHardStoppingState HardStoppingState { get; }
        public PlayerJumpingState JumpingState { get; }
        public PlayerJumpDashState JumpDashState { get; }
        public PlayerFallingState FallingingState { get; }
        public PlayerLightLandingState LightLandingState { get; }
        public PlayerRollingState RollingState { get; }
        public PlayerHardLandingState HardLandingState { get; }
        public PlayerWallRunningState WallRunningState { get; }
        public PlayerClimbingIdleingState ClimbingHighState { get; }
        public PlayerSuiSeiJumpState SuiSeiJumpState { get; }
        public PlayerJumpPadsState JumpPadsState { get; }
        public PlayerSuiSeiFallState SuiSeiFallState { get; }

        

        public PlayerMovementStateMachine(Player player)
        {
            Player = player;
            ReusableData = new PlayerStateReusableData();
            RespawnState = new PlayerRespawnState(this);
            DeathState = new PlayerDeathState(this);
            IdleingState = new PlayerIdleingState(this);
            DashingState = new PlayerDashingState(this);

            WalkingState = new PlayerWalkingState(this);
            RunningState = new PlayerRunningState(this);
            SprintingState = new PlayerSprintingState(this);

            LightStoppingState = new PlayerLightStoppingState(this);
            MediumStoppingState = new PlayerMediumStoppingState(this);
            HardStoppingState = new PlayerHardStoppingState(this);

            LightLandingState = new PlayerLightLandingState(this);
            RollingState = new PlayerRollingState(this);
            HardLandingState = new PlayerHardLandingState(this);

            JumpingState = new PlayerJumpingState(this);
            JumpDashState = new PlayerJumpDashState(this);
            FallingingState = new PlayerFallingState(this);
            SuiSeiJumpState = new PlayerSuiSeiJumpState(this);
            SuiSeiFallState = new PlayerSuiSeiFallState(this);
            JumpPadsState = new PlayerJumpPadsState(this);

            WallRunningState = new PlayerWallRunningState(this);
            ClimbingHighState = new PlayerClimbingIdleingState(this);
        }
    }
}


