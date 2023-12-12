using System.Collections;
using System;
using UnityEngine;

namespace Nomimovment
{
    [Serializable]
    public class PlayerTriggerColliderData
    {
        [field: SerializeField] public BoxCollider GroundCheckCollider { get; private set; }
        [field: SerializeField] public BoxCollider LedgeCheckCollider { get; private set; }
        public Vector3 GroundCheckColliderVerticalExtents { get; private set; }

        public void Initialize()
        {
            GroundCheckColliderVerticalExtents = GroundCheckCollider.bounds.extents;
        }

    }
}

