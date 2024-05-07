using System;
using System.Collections.Generic;
using UnityEngine;
namespace Movement 
{
    [Serializable]
    public class PlayerAnimationData
    {
        [Header("State Group Parmeter Names")]
        [SerializeField] private string DieParmeterNames = "Die";
        [SerializeField] private string RespawnParmeterNames = "Respawn";
        [SerializeField] private string GroundParmeterNames = "Ground";
        [SerializeField] private string AirParmeterNames = "Air";
        [SerializeField] private string ClimbParmeterNames = "Climb";
        [SerializeField] private string MovingstateParmeterNames = "Moving";
        [SerializeField] private string StopstateParmeterNames = "Stop";
        [SerializeField] private string LandingstateParmeterNames = "Landing";
        [SerializeField] private string IdlingstateParmeterNames = "IsIdling";


        [Header("Idling Parmeter Names")]
        [SerializeField] private string IdlestateParmeterNames = "idlestate";

        [Header("Dash Parmeter Names")]
        [SerializeField] private string DashstateParmeterNames = "Dash";

        [Header("Moving Parmeter Names")]
        [SerializeField] private string MovestateParmeterNames = "movestate";

        [Header("Stop Parmeter Names")]
        [SerializeField] private string MediumstateParmeterNames = "MediumStop";
        [SerializeField] private string HardstateParmeterNames = "HardStop";
        [SerializeField] private string StopParmeterNames = "stoptransform";

        [Header("Jump Parmeter Names")]
        [SerializeField] private string VerticalspeedParmeterNames = "verticalspeed";
        [SerializeField] private string FeetParmeterNames = "feet";
        [SerializeField] private string JumpDashNames = "AirDash";
        [SerializeField] private string SuiSeiNames = "SuiSei";

        [Header("Land Parmeter Names")]
        [SerializeField] private string HardLandstateParmeterNames = "HardLand";
        [SerializeField] private string RollstateParmeterNames = "Roll";

        [Header("Climb Parmeter Names")]
        [SerializeField] private string ClimbUpParmeterNames = "ClimbUp";


        public int DiestateHash { get; private set; }
        public int RespawnstateHash { get; private set; }
        public int GroundstateHash { get; private set; }
        public int AirstateHash { get; private set; }
        public int ClimbstateHash { get; private set; }
        public int IsStopHash { get; private set; }
        public int IsMoveHash { get; private set; }
        public int IsLandHash { get; private set; }

        public int MoveSpeedHash { get; private set; }
        public int IsDashHash { get; private set; }

        public int MediumStopHash { get; private set; }
        public int HardStopHash { get; private set; }
        public int stoptransformHash { get; private set; }



        public int VerticalVelHash { get; private set; }
        public int FeetTweenHash { get; private set; }
        public int JumpDashHash { get; private set; }
        public int SuiSeiHash { get; private set; }

        public int IdlestateHash { get; private set; }
        public int IsIdleHash { get; private set; }

        public int HardLandHash { get; private set; }
        public int RollLandHash { get; private set; }

        public int ClimbUpstateHash { get; private set; }





        //public float GroundThreshold { get; private set; } = 1f;
        //public float JumpThreshold { get; private set; } = 2.1f;
        //public float LandingThreshold { get; private set; } = 1f;


        public void Intialoze()
        {
            DiestateHash = Animator.StringToHash(DieParmeterNames);
            RespawnstateHash = Animator.StringToHash(RespawnParmeterNames);
            GroundstateHash = Animator.StringToHash(GroundParmeterNames);
            AirstateHash = Animator.StringToHash(AirParmeterNames);
            ClimbstateHash = Animator.StringToHash(ClimbParmeterNames);
            MoveSpeedHash = Animator.StringToHash(MovestateParmeterNames);
            IsMoveHash = Animator.StringToHash(MovingstateParmeterNames);
            IsStopHash = Animator.StringToHash(StopstateParmeterNames);
            IsDashHash = Animator.StringToHash(DashstateParmeterNames);
            IsLandHash = Animator.StringToHash(LandingstateParmeterNames);

            MediumStopHash = Animator.StringToHash(MediumstateParmeterNames);
            HardStopHash = Animator.StringToHash(HardstateParmeterNames);
            stoptransformHash = Animator.StringToHash(StopParmeterNames);

            VerticalVelHash = Animator.StringToHash(VerticalspeedParmeterNames);
            FeetTweenHash = Animator.StringToHash(FeetParmeterNames);
            JumpDashHash = Animator.StringToHash(JumpDashNames);
            SuiSeiHash = Animator.StringToHash(SuiSeiNames);

            IdlestateHash = Animator.StringToHash(IdlestateParmeterNames);
            IsIdleHash = Animator.StringToHash(IdlingstateParmeterNames);

            HardLandHash = Animator.StringToHash(HardLandstateParmeterNames);
            RollLandHash = Animator.StringToHash(RollstateParmeterNames);

            ClimbUpstateHash = Animator.StringToHash(ClimbUpParmeterNames);
        }
    }
}

