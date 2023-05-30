using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Player
{
#if UNITY_EDITOR
    [DisallowMultipleComponent, RequireComponent(typeof(PhotonView))]
#endif
    public sealed class Nickname : MonoBehaviour
    {
        #region Editor Fields
        [SerializeField] private TextMeshPro _nickname = null;
        #endregion

        #region Fields
        private PhotonView _view = null;
        #endregion

        private void Awake()
        {
            _view = GetComponent<PhotonView>();
        }

        private void Start()
        {
            if (_view.IsMine)
            {
                _nickname.gameObject.SetActive(false);
            }
            else
            {
                _nickname.text = _view.Owner.NickName;
            }
        }
    }
}