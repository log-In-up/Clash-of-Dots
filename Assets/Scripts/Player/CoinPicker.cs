using Photon.Pun;
using UnityEngine;

namespace Player
{
#if UNITY_EDITOR
    [DisallowMultipleComponent]
#endif
    public sealed class CoinPicker : MonoBehaviour
    {
        #region Fields
        private PhotonView _view = null;
        private PlayerManager _playerManager = null;

        private const string COIN_TAG = "Coin";
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            _view = GetComponent<PhotonView>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(COIN_TAG) && collision.TryGetComponent(out PhotonView photonView))
            {
                _playerManager.AddCoin();

                _view.RPC(nameof(PRC_DestroyObject), RpcTarget.MasterClient, photonView.ViewID);
            }
        }
        #endregion

        #region Public Methods
        internal void Init(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        #endregion

        #region Remote Procedure Calls
        [PunRPC]
        private void PRC_DestroyObject(int viewID)
        {
            PhotonView photonView = PhotonView.Find(viewID);

            PhotonNetwork.Destroy(photonView);
        }
        #endregion
    }
}