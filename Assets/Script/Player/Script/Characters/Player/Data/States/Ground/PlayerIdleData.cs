using System.Collections.Generic;
using System;
using UnityEngine;

namespace Nomimovment
{
    [Serializable]
    public class PlayerIdleData
    {
        [field: SerializeField] public List<PlayerCameraRecenteringData> BackwardsCameraRecenteringData { get; private set; }
    }
}

