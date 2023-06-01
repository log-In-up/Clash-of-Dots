using ExitGames.Client.Photon;
using GameData;
using Photon.Pun;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInterface;
using Utility;
using PhotonPlayer = Photon.Realtime.Player;

namespace Player
{
#if UNITY_EDITOR
    [DisallowMultipleComponent, RequireComponent(typeof(PhotonView))]
#endif
    public sealed class PlayerManager : MonoBehaviourPunCallbacks
    {
        #region Editor Fields
        [SerializeField] private PopupHandler _popupHandler = null;
        [SerializeField] private PlayerData _playerData = null;
        [SerializeField] private GameObject _userInterface = null;
        [SerializeField] private Joystick _movementJoystick = null;
        [SerializeField] private Joystick _rotationJoystick = null;
        [SerializeField] private Slider _healthBar = null;
        [SerializeField] private TextMeshProUGUI _coins = null;
        #endregion

        #region Fields
        private ulong _coinsAmount;
        private GameObject _controller = null;
        private PhotonView _view = null;
        private Health _health = null;
        private Movement _playerMovement = null;
        private Shooter _shooter = null;
        private const string PHOTON_PREFABS_FOLDER = "Photon Prefabs", PLAYER_CONTROLLER_PREFAB_NAME = "Player", NICKNAME = "Nickname", COINS = "Coins", IS_ALIVE = "IsAlive";
        #endregion

        #region Properties
        public bool IsAlive => (bool)_view.Owner.CustomProperties[IS_ALIVE];
        public string Nickname => _view.Owner.NickName;
        public ulong Coins => _coinsAmount;
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            _view = GetComponent<PhotonView>();
        }

        private void Start()
        {
            if (_view.IsMine)
            {
                _popupHandler.gameObject.SetActive(false);
                CreateController();
            }
            else
            {
                Destroy(_userInterface);
            }
        }
        #endregion

        #region PunCallbacks
        public override void OnPlayerEnteredRoom(PhotonPlayer newPlayer)
        {
            UpdateMovement();
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (!_view.IsMine) return;

            if ((string)PhotonNetwork.CurrentRoom.CustomProperties[NICKNAME] != null)
            {
                string nickname = (string)PhotonNetwork.CurrentRoom.CustomProperties[NICKNAME];
                int coins = (int)PhotonNetwork.CurrentRoom.CustomProperties[COINS];

                _popupHandler.ShowPopup(nickname, coins);
                _popupHandler.gameObject.SetActive(true);
            }
        }

        public override void OnPlayerPropertiesUpdate(PhotonPlayer targetPlayer, Hashtable changedProps)
        {
            if (!_view.IsMine) return;
            if (_playerMovement.MovementIsLocked) return;

            PlayerManager[] alivePlayers = Array.FindAll(FindObjectsOfType<PlayerManager>(), p => p.IsAlive == true);
            if (alivePlayers.Length < 2)
            {
                PlayerManager player = alivePlayers[0];

                Hashtable setValue = new Hashtable
                {
                    { NICKNAME, player.Nickname },
                    { COINS, (int)player.Coins }
                };
                PhotonNetwork.CurrentRoom.SetCustomProperties(setValue);
            }
        }
        #endregion

        #region Methods
        private void CreateController()
        {
            string prefabName = Path.Combine(PHOTON_PREFABS_FOLDER, PLAYER_CONTROLLER_PREFAB_NAME);
            byte group = 0;
            object[] data = new object[] { _view.ViewID };

            Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
            _controller = PhotonNetwork.Instantiate(prefabName, spawnpoint.position, spawnpoint.rotation, group, data);

            _controller.GetSafeComponent(out Movement movement);
            _playerMovement = movement;
            movement.Init(_movementJoystick, _rotationJoystick, _playerData.SmoothMovementTime, _playerData.MovementSpeed);
            UpdateMovement();

            _controller.GetSafeComponent(out Health health);
            _health = health;
            health.Init(this, _healthBar, _playerData.MaxHealth);
            Hashtable setValue = new Hashtable { { IS_ALIVE, true } };
            _view.Owner.SetCustomProperties(setValue);

            _controller.GetSafeComponent(out Shooter shooter);
            _shooter = shooter;

            _controller.GetSafeComponent(out CoinPicker coinPicker);
            coinPicker.Init(this);
        }

        private void UpdateMovement()
        {
            if (_playerMovement.MovementIsLocked)
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
                {
                    _playerMovement.MovementIsLocked = false;
                }
            }
        }
        #endregion

        #region Public Methods
        internal void OnPlayerDeath()
        {
            PhotonNetwork.Destroy(_controller);

            Hashtable setValue = new Hashtable
            {
                { IS_ALIVE, false },
                { COINS, (int)_coinsAmount }
            };
            _view.Owner.SetCustomProperties(setValue);
        }

        internal void PerformShoot()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                if (_shooter != null)
                {
                    _shooter.PerformShoot();
                }
            }
        }

        internal void AddCoin()
        {
            _view.RPC(nameof(RPC_AddCoin), RpcTarget.All);

            _coins.text = _coinsAmount.ToString();
        }
        #endregion

        #region Remote Procedure Calls
        [PunRPC]
        private void RPC_AddCoin()
        {
            _coinsAmount++;
        }
        #endregion
    }
}