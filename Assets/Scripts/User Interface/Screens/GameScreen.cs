using Photon.Pun;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
#if UNITY_EDITOR
    [DisallowMultipleComponent]
#endif
    public sealed class GameScreen : MonoBehaviourPunCallbacks
    {
        #region Editor Fields
        [SerializeField] private Button _shootButton = null;
        [SerializeField] private PlayerManager _playerManager = null;
        #endregion

        #region PunCallbacks
        public override void OnEnable()
        {
            _shootButton.onClick.AddListener(OnClickShootButton);

            base.OnEnable();
        }

        public override void OnDisable()
        {
            _shootButton.onClick.RemoveListener(OnClickShootButton);

            base.OnDisable();
        }
        #endregion

        #region Event Handlers
        private void OnClickShootButton()
        {
            _playerManager.PerformShoot();
        }
        #endregion
    }
}