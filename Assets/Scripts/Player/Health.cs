using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
#if UNITY_EDITOR
    [DisallowMultipleComponent, RequireComponent(typeof(PhotonView))]
#endif
    public sealed class Health : MonoBehaviour
    {
        #region Fields
        private PhotonView _view = null;
        private Slider _healthBar = null;
        private float _currentHealth, _maxHealth;
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            _view = GetComponent<PhotonView>();
        }
        #endregion

        #region Public Methods
        internal void Init(Slider healthBar, float maxHealth)
        {
            _healthBar = healthBar;
            _currentHealth = _maxHealth = maxHealth;

            _healthBar.value = _currentHealth / _maxHealth;
        }

        internal void ApplyDamage(float damage)
        {
            _view.RPC(nameof(RPC_ApplyDamage), RpcTarget.All, damage);
        }
        #endregion

        #region Methods
        [PunRPC]
        private void RPC_ApplyDamage(float damage)
        {
            if (!_view.IsMine) return;

            _currentHealth -= damage;

            _healthBar.value = _currentHealth / _maxHealth;

            if (_currentHealth <= 0.0f)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        #endregion
    }
}