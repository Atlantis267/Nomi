using UnityEngine;

namespace Movement
{
    public class PlayerStateReusableData
    {
        public Vector2 CurrentMovementInput { get; set; }
        public float SpeedMultiplier { get; set; } = 1f;
        public float PlayerHeight { get; set; } 
        public float MovementDelcelerationForce { get; set; } = 1f;
        public float FallMulitiplier { get; set; } = 1f;
        public float Gravity { get; set; } = -35.8f;
        public float SlopeGravity { get; set; } = -9.8f;
        public float GroundedGravity { get; set; } = -0.21f;
        public float GroundCheckOffset { get; set; } = 0.5f;



        public bool OnLedge { get; set; }
        public bool IsRespawning { get; set; }
        public bool IscloseingDir { get; set; }
        public bool NearStar { get; set; }
        public bool ShouldWalk { get; set; }
        public bool ShouldSprint { get; set; }
        public bool KeepSprint { get; set; }
        public bool ShouldDash { get; set; }
        public bool ShouldAirDash { get; set; }
        public bool ShouldSuiseiJump { get; set; }
        public bool SuiseiJumpFinish { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsAirDashing { get; set; }
        public bool IsSliding { get; set; }
        public bool IsDashing { get; set; }
        public bool IsSprinting { get; set; }
        public float currenttargetRotation { get; set; } = 0.0f;
        public float VerticalVelocity { get; set; }
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
        public Vector3 SuiSeiJumpDir { get; set; }
        public RaycastHit rayFindLedge;
        public RaycastHit faceLedge;
        public Vector3 playerOffect = new Vector3(0f, -1.74f, -0.36f);
        public PlayerRotationData RotatonData { get; set; }
    }

}

