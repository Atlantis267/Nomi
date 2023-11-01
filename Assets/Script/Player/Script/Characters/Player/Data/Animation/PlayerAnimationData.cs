using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class PlayerAnimationData
{
    [Header("State Group Parmeter Names")]
    [SerializeField] private string GroundParmeterNames = "Ground";
    [SerializeField] private string AirParmeterNames = "Air";
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

    [Header("Land Parmeter Names")]
    [SerializeField] private string HardLandstateParmeterNames = "HardLand";
    [SerializeField] private string RollstateParmeterNames = "Roll";


    public int GroundstateHash { get; private set; }
    public int AirstateHash { get; private set; }
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

    public int IdlestateHash { get; private set; }
    public int IsIdleHash { get; private set; }

    public int HardLandHash { get; private set; }
    public int RollLandHash { get; private set; }





    //public float GroundThreshold { get; private set; } = 1f;
    //public float JumpThreshold { get; private set; } = 2.1f;
    //public float LandingThreshold { get; private set; } = 1f;


    public void Intialoze()
    {
        GroundstateHash = Animator.StringToHash(GroundParmeterNames);
        AirstateHash = Animator.StringToHash(AirParmeterNames);
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

        IdlestateHash = Animator.StringToHash(IdlestateParmeterNames);
        IsIdleHash = Animator.StringToHash(IdlingstateParmeterNames);

        HardLandHash = Animator.StringToHash(HardLandstateParmeterNames);
        RollLandHash = Animator.StringToHash(RollstateParmeterNames);
    }




}
