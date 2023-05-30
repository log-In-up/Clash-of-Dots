using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public sealed class Health : MonoBehaviour
    {
        #region Fields
        private Slider _healthBar = null;
        private float _currentHealth, _maxHealth;
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