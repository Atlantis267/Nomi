using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nomimovment
{
    public class PlayerMovementStateMachine : StateMachine
    {
        public Player Player { get; }
        public PlayerStateReusableData ReusableData { get; }
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
        public PlayerFallingState FallingState { get; }
        public PlayerLightLandingState LightLandingState { get; }
        public PlayerRollingState RollingState { get; }
        public PlayerHardLandingState HardLandingState { get; }


        public PlayerMovementStateMachine(Player player)
        {
            Player = player;
            ReusableData = new PlayerStateReusableData();
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
            FallingState = new PlayerFallingState(this);


        }
    }
}


