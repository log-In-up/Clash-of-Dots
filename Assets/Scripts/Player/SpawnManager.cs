using UnityEngine;

namespace Player
{
#if UNITY_EDITOR
    [DisallowMultipleComponent]
#endif
    public sealed class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance;

        private Transform[] _spawnpoints = null;

        private void Awake()
        {
            Instance = this;
            _spawnpoints = GetComponentsInChildren<Transform>();
        }

        public Transform GetSpawnpoint()
        {
            return _spawnpoints[Random.Range(0, _spawnpoints.Length)];
        }
    }
}