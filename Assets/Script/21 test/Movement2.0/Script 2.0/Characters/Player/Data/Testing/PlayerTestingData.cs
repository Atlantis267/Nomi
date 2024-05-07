using System;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    [Serializable]
    public class PlayerTestingData
    {
        [field: SerializeField] public Transform wallTransform { get; private set; }
        [field: SerializeField] public GameObject rayhitmark { get; private set; }
        [field: SerializeField] public Transform headHeight { get; private set; }
    }
}
