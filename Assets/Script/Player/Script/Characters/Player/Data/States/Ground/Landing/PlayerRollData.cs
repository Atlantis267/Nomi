using System;
using System.Collections.Generic;
using UnityEngine;


namespace Nomimovment
{
    [Serializable]
    public class PlayerRollData
    {
        [field: SerializeField] [field: Range(0f, 3f)] public float SpeedModifier { get; private set; } = 1f;
    }
}

