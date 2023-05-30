using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Application;

namespace Network
{
#if UNITY_EDITOR
    [DisallowMultipleComponent]
#endif
    public sealed class NetworkConnector : MonoBehaviourPunCallbacks
    {
        #region MonoBehaviour API
        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        #endregion

        #region Pun Callbacks
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
#if UNITY_EDITOR
            Debug.Log("Connected to Master. Joining the lobby...");
#endif
        }

        public override void OnJoinedLobby()
        {
            SceneManager.LoadScene((int)SceneIndex.Lobby);
#if UNITY_EDITOR
            Debug.Log("Joined Lobby.");
#endif
        }
        #endregion
    }
}