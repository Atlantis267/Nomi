using System;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    [Serializable]
    public class PlayerRotationData
    {
        [field: SerializeField] public float RotationSmoothTime { get; private set; } = 0.12f;
    }
}

