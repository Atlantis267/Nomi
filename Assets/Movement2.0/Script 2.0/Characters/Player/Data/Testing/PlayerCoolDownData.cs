using System;
using UnityEngine;

namespace Movement
{
    [Serializable]
    public class PlayerCoolDownData
    {
        [field: SerializeField] public float climbidlecooldownTime;
        [field: SerializeField] public float airdashcooldownTime;
        [field: SerializeField] public float suiseijumpcooldownTime;
        private float _climbidlenextTime;
        private float _airdashnextTime;
        private float _suiseinextTime;

        public bool IsClimbIdleingCoolingDown => Time.time < _climbidlenextTime;
        public bool IsAirDashCoolingDown => Time.time < _airdashnextTime;
        public bool IsSuiSeiJumpCoolingDown => Time.time < _suiseinextTime;

        public void StartClimbIdleingCooldown() => _climbidlenextTime = Time.time + climbidlecooldownTime;

        public void StartAirDashCooldown() => _airdashnextTime = Time.time + airdashcooldownTime;
        public void StartSuiSeiJumpCooldown() => _suiseinextTime = Time.time + suiseijumpcooldownTime;
    }
}
