using GameData;
using Photon.Pun;
using Player;
using UnityEngine;

namespace Projectile
{
#if UNITY_EDITOR
    [DisallowMultipleComponent, RequireComponent(typeof(PhotonView))]
#endif
    public class Rocket : MonoBehaviour
    {
        #region Editor Fields
        [SerializeField] private ProjectileData _projectileData = null;
        #endregion

        #region Fields
        private PhotonView _view = null;

        private const string PLAYER_TAG = "Player";
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            _view = GetComponent<PhotonView>();
        }

        private void Update()
        {
            if (!_view.IsMine) return;

            Vector3 translation = _projectileData.MovementSpeed * Time.deltaTime * Vector3.right;
            transform.Translate(translation);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(PLAYER_TAG) && collision.gameObject.TryGetComponent(out Health health))
            {
                health.ApplyDamage(_projectileData.Damage);
            }
        }
        #endregion
    }
}