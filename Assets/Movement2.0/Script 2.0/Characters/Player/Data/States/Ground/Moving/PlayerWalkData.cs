using System;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    [Serializable]
    public class PlayerWalkData
    {
        [field: SerializeField] [field: Range(0f, 1f)] public float SpeedModifier { get; private set; } = 0.5f;
    }
}

