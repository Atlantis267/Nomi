using System;
using System.Collections.Generic;
using UnityEngine;


namespace Nomimovment
{
    [Serializable]

    public class PlayerAirborneData
    {
        [field: SerializeField] public PlayerJumpData JumpData { get; private set; }
        [field: SerializeField] public PlayerFallData FallData { get; private set; }
    }
}

