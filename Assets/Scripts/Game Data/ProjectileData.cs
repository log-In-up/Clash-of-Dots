using UnityEngine;

namespace GameData
{
    [CreateAssetMenu(fileName = "Projectile Data", menuName = "Game Data/Projectiles/Projectile", order = 0)]
    public sealed class ProjectileData : ScriptableObject
    {
        [field: SerializeField, Min(0.0f)] public float MovementSpeed { get; private set; }
        [field: SerializeField, Min(0.0f)] public float Damage { get; internal set; }
    }
}