using System;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    [Serializable]
    public class PlayerFallData
    {
        [field: SerializeField] [field: Range(0f, 5f)] public float FallMultiplier { get; private set; } = 2f;
        [field: SerializeField] [field: Range(0f, 100f)] public float MinimumDistanceHardFall { get; private set; } = 3f;

    }
}

