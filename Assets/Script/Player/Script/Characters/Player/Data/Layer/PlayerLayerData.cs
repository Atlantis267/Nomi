using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nomimovment
{
    [Serializable]
    public class PlayerLayerData
    {
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
        [field: SerializeField] public LayerMask LedgeLayer { get; private set; }
        public bool ContainsLayer(LayerMask layerMask, int layer)
        {
            return (1 << layer & layerMask) != 0;
        }
        public bool IsGroundLayer(int layer)
        {
            return ContainsLayer(GroundLayer, layer);
        }
        public bool IsLedgeLayer(int layer)
        {
            return ContainsLayer(LedgeLayer, layer);
        }
    }
}

