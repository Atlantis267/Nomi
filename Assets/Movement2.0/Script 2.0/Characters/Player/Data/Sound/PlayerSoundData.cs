using System;
using System.Collections.Generic;
using UnityEngine;


namespace Movement
{
    [Serializable]
    public class PlayerSoundData 
    {
        [field: SerializeField] public List<AudioClip> WalkSound { get; private set; }
        [field: SerializeField] public AudioSource audioSource { get; private set; }
    }
}

