using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nomimovment
{
    [Serializable]

    public class PlayerParticalData
    {
        [field: SerializeField] public ParticleSystem FlashBarrier { get; private set; }
        [field: SerializeField] public ParticleSystem SmokeParticle { get; private set; }
    }
}

