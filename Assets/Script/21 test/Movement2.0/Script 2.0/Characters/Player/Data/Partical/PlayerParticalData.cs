using System;
using System.Collections.Generic;
using UnityEngine;

namespace Movement 
{
    [Serializable]

    public class PlayerParticalData
    {
        [field: SerializeField] public ParticleSystem FlashBarrier { get; private set; }
        [field: SerializeField] public ParticleSystem SmokeParticle { get; private set; }
        [field: SerializeField] public ParticleSystem HeatParticle { get; private set; }
        [field: SerializeField] public ParticleSystem CameraParticle { get; private set; }
    }

}
