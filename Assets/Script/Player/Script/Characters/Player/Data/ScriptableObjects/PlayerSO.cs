using UnityEngine;


namespace Nomimovment
{
    [CreateAssetMenu(fileName = "Player", menuName = "Movement System/Characters/Player")]
    public class PlayerSO : ScriptableObject
    {
        [field: SerializeField] public PlayerGroundData GroundedData { get; private set; }
        [field: SerializeField] public PlayerAirborneData AirborneData { get; private set; }

    }
}

