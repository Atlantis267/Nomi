using System;
using System.Collections.Generic;
using UnityEngine;

namespace Movement 
{
    [Serializable]
    public class PlayerLayerData
    {
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
        [field: SerializeField] public LayerMask JumpPadsLayer { get; private set; }
        [field: SerializeField] public LayerMask WallLayer { get; private set; }
        public bool ContainsLayer(LayerMask layerMask, int layer)
        {
            return (1 << layer & layerMask) != 0;
        }
        public bool IsGroundLayer(int layer)
        {
            return ContainsLayer(GroundLayer, layer);
        }
    }
}

