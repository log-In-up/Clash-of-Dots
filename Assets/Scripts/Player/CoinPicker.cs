using Photon.Pun;
using System;
using UnityEngine;

namespace Player
{
#if UNITY_EDITOR
    [DisallowMultipleComponent]
#endif
    public sealed class CoinPicker : MonoBehaviour
    {
        #region Fields
        private PlayerManager _playerManager = null;

        private const string COIN_TAG = "Coin";
        #endregion

        #region MonoBehaviour API
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag(COIN_TAG))
            {
                _playerManager.AddCoin();

                PhotonNetwork.Destroy(collision.gameObject);
            }
        }

        internal void Init(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        #endregion
    }
}