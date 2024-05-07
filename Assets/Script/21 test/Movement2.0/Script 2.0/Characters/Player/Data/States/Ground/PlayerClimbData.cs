using System;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    [Serializable]
    public class PlayerClimbData
    {
        [field: SerializeField] [field: Range(0f, 3f)] public float LowClimbHeight { get; private set; } = 0.5f;
        [field: SerializeField] [field: Range(1f, 5f)] public float HighClimbHeight { get; private set; } = 1.6f;
        [field: SerializeField] [field: Range(0f, 5f)] public float CheckDistance { get; private set; } = 1f;
        [field: SerializeField] [field: Range(0f, 360f)] public float ClimbAngle { get; private set; } = 45f;
        [field: SerializeField] [field: Range(1f, 3f)] public float BodyHeight { get; private set; } = 1f;
        [field: SerializeField] [field: Range(0f, 2f)] public float ValutDistance { get; private set; } = 0.2f;
        [field: SerializeField] [field: Range(0f, 2f)] public float MovableObjectHeight { get; private set; } = 0.8f;
    }
}

