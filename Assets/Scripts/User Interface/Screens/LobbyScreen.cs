using Application;
using Photon.Pun;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UserInterface
{
#if UNITY_EDITOR
    [DisallowMultipleComponent]
#endif
    public sealed class LobbyScreen : MonoBehaviourPunCallbacks
    {
        #region Editor Fields
        [SerializeField] private Button _createRoom = null;
        [SerializeField] private Button _enterRoom = null;
        [SerializeField] private Button _randomNickname = null;
        [SerializeField] private TMP_InputField _createRoomInputField = null;
        [SerializeField] private TMP_InputField _enterRoomInputField = null;
        [SerializeField] private NicknameGenerator _nicknameGenerator = null;
        #endregion

        #region MonoBehaviour API
        public override void OnEnable()
        {
            _createRoom.onClick.AddListener(OnClickCreateRoom);
            _enterRoom.onClick.AddListener(OnClickEnterRoom);
            _randomNickname.onClick.AddListener(OnClickRandomNickname);

            base.OnEnable();
        }

        private void Start()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnDisable()
        {
            _createRoom.onClick.RemoveListener(OnClickCreateRoom);
            _enterRoom.onClick.RemoveListener(OnClickEnterRoom);
            _randomNickname.onClick.RemoveListener(OnClickRandomNickname);

            base.OnDisable();
        }
        #endregion

        #region Event Handlers
        private void OnClickCreateRoom()
        {
            if (string.IsNullOrEmpty(_createRoomInputField.text)) return;

            PhotonNetwork.CreateRoom(_createRoomInputField.text);
        }

        private void OnClickEnterRoom()
        {
            if (string.IsNullOrEmpty(_enterRoomInputField.text)) return;

            PhotonNetwork.JoinRoom(_enterRoomInputField.text);
        }

        private void OnClickRandomNickname()
        {
            _nicknameGenerator.SetupRandomNickname();
        }
        #endregion

        #region Pun Callbacks
        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel((int)SceneIndex.Game);
        }
        #endregion
    }
}