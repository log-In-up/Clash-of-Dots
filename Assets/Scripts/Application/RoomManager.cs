using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Application
{
#if UNITY_EDITOR
    [DisallowMultipleComponent]
#endif
    public sealed class RoomManager : MonoBehaviourPunCallbacks
    {
        #region Fields
        private const string PHOTON_PREFABS_FOLDER = "Photon Prefabs", PLAYER_MANAGER_PREFAB_NAME = "Player Manager";
        #endregion

        #region Pun Callbacks
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public override void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            base.OnEnable();
        }

        public override void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            base.OnDisable();
        }
        #endregion

        #region Event Handlers
        private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.buildIndex.Equals((int)SceneIndex.Game))
            {
                PhotonNetwork.Instantiate(Path.Combine(PHOTON_PREFABS_FOLDER, PLAYER_MANAGER_PREFAB_NAME), Vector3.zero, Quaternion.identity);
            }
        }
        #endregion
    }
}