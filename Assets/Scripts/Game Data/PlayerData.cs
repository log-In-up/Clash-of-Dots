using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "Player Data", menuName = "Game Data/Player/Data", order = 0)]
    public sealed class PlayerData : ScriptableObject
    {
        [field: SerializeField, Min(0.0f)] public float MaxHealth { get; private set; }
        [field: SerializeField, Min(0.0f)] public float SmoothMovementTime { get; private set; }
        [field: SerializeField, Min(0.0f)] public float MovementSpeed { get; private set; }
    }
}