using Photon.Pun;
using System.IO;
using UnityEngine;

namespace Player
{
#if UNITY_EDITOR
    [DisallowMultipleComponent, RequireComponent(typeof(PhotonView))]
#endif
    public sealed class Shooter : MonoBehaviour
    {
        #region Editor Fields
        [SerializeField] private Transform _shootingPoint = null;
        #endregion

        #region Fields
        private PhotonView _view = null;

        private const string PHOTON_PREFABS_FOLDER = "Photon Prefabs", PROJECTILE_PREFAB_NAME = "Projectile";
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            _view = GetComponent<PhotonView>();
        }
        #endregion

        #region Public Methods
        internal void PerformShoot()
        {
            string prefabName = Path.Combine(PHOTON_PREFABS_FOLDER, PROJECTILE_PREFAB_NAME);
            byte group = 0;
            object[] data = new object[] { _view.ViewID };

            PhotonNetwork.Instantiate(prefabName, _shootingPoint.position, _shootingPoint.rotation, group, data);
        }
        #endregion
    }
}