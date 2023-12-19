using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nomimovment
{
    [Serializable]
    public class DefaultColliderData
    {
        [field: SerializeField] public float Height { get; private set; } = 1.95f;
        [field: SerializeField] public float CenterY { get; private set; } = 1f;
        [field: SerializeField] public float Radius { get; private set; } = 0.2f;
    }
}


