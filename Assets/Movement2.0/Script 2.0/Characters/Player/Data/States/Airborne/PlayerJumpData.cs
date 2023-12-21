using System;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    [Serializable]
    public class PlayerJumpData
    {
        [field: SerializeField] public float JumpForce { get; set; }
        [field: SerializeField] public float JumpDashForce { get; set; }
        [field: SerializeField]  public Vector3 StationaryForce { get; private set; }
        [field: SerializeField] public Vector3 WeekForce { get; private set; }
        [field: SerializeField] public Vector3 MediumForce { get; private set; }
        [field: SerializeField] public Vector3 StrongForce { get; private set; }       

    }
}

