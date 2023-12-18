using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Movement
{
    [Serializable]

    public class PlayerUIData
    {
        [field: SerializeField] public Slider boostSlider { get; private set; }
    }
}


