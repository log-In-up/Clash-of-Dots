using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Player
{
#if UNITY_EDITOR
    [DisallowMultipleComponent]
#endif
    public sealed class NicknameGenerator : MonoBehaviour
    {
        #region Editor Fields
        [SerializeField] private TMP_InputField _usernameInput = null;
        #endregion

        #region Fields
        private const string USERNAME_KEY = "Username";
        #endregion

        #region MonoBehaviour API
        private void OnEnable()
        {
            _usernameInput.onEndEdit.AddListener(OnEndEditUsername);
        }

        private void Start()
        {
            SetupPlayerNickname();
        }

        private void OnDisable()
        {
            _usernameInput.onEndEdit.RemoveListener(OnEndEditUsername);
        }
        #endregion

        #region Methods
        private void SetupPlayerNickname()
        {
            if (!PlayerPrefs.HasKey(USERNAME_KEY))
            {
                SetupRandomNickname();
            }
            else
            {
                _usernameInput.text = PlayerPrefs.GetString(USERNAME_KEY);
                PhotonNetwork.NickName = PlayerPrefs.GetString(USERNAME_KEY);
            }
        }
        #endregion

        #region Public Methods
        public void SetupRandomNickname()
        {
            string nickname = $"Player {Random.Range(0, 1000):0000}";

            _usernameInput.text = nickname;
            PhotonNetwork.NickName = nickname;
            PlayerPrefs.SetString(USERNAME_KEY, nickname);
        }
        #endregion

        #region Event Handlers
        private void OnEndEditUsername(string text)
        {
            PhotonNetwork.NickName = text;
            PlayerPrefs.SetString(USERNAME_KEY, text);
        }
        #endregion
    }
}