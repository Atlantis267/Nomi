using System;
using System.Collections.Generic;
using UnityEngine;


namespace Movement
{
    [Serializable]
    public class PlayerSoundlData
    {
        [field: SerializeField] public AudioSource FootStepsSound { get; private set; }
        [field: SerializeField] public AudioSource JumpSound { get; private set; }
        [field: SerializeField] public AudioSource DashSound { get; private set; }
    }
}

