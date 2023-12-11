using UnityEngine;
using System.Collections.Generic;

namespace Nomimovment
{
    public class PlayerStateReusableData
    {
        public Vector2 CurrentMovementInput { get; set; }
        public float SpeedMultiplier { get; set; } = 1f;
        public float SlopeSpeedMultiplier { get; set; } = 1f;
        public float MovementDelcelerationForce { get; set; } = 1f;

        public List<PlayerCameraRecenteringData> SidewaysCameraRecenteringData { get; set; }
        public List<PlayerCameraRecenteringData> BackwardsCameraRecenteringData { get; set; }


        public float GroundCheckOffset { get; set; } = 0.5f;
        public bool ShouldWalk { get; set; }
        public bool ShouldSprint { get; set; }
        public bool KeepSprint { get; set; }
        public bool ShouldDash { get; set; }
        public bool ShouldAirDash { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsAirDashing { get; set; }
        public bool IsSliding { get; set; }
        public bool IsDashing { get; set; }
        public bool IsSprinting { get; set; }
        public float currenttargetRotation { get; set; } = 0.0f;
        public float RotationSmoothTime { get; set; } = 0.12f;
        private Vector3 rotationVelocity;
        private Vector3 currentMovement;

        public ref Vector3 RotationVelocity
        {
            get
            {
                return ref rotationVelocity;
            }
        }
        public ref Vector3 CurrentMovement
        {
            get
            {
                return ref currentMovement;
            }
        }
        public Vector3 CurrentJumpForce { get; set; }
        public PlayerRotationData RotatonData { get; set; }
    }
}

