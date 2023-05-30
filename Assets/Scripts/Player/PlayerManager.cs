using GameData;
using Photon.Pun;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace Player
{
#if UNITY_EDITOR
    [DisallowMultipleComponent, RequireComponent(typeof(PhotonView))]
#endif
    public sealed class PlayerManager : MonoBehaviour
    {
        #region Editor Fields
        [SerializeField] private PlayerData _playerData = null;
        [SerializeField] private GameObject _userInterface = null;
        [SerializeField] private Joystick _movementJoystick = null;
        [SerializeField] private Joystick _rotationJoystick = null;
        [SerializeField] private Slider _healthBar = null;
        [SerializeField] private TextMeshProUGUI _coins = null;
        #endregion

        #region Fields
        private Shooter _shooter = null;
        private PhotonView _view = null;
        private const string PHOTON_PREFABS_FOLDER = "Photon Prefabs", PLAYER_CONTROLLER_PREFAB_NAME = "Player";
        #endregion

        #region Fields
        private ulong _coinsAmount;
        #endregion

        #region Properties

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
                CreateController();
            }
            else
            {
                Destroy(_userInterface);
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
            GameObject controller = PhotonNetwork.Instantiate(prefabName, spawnpoint.position, spawnpoint.rotation, group, data);

            controller.GetSafeComponent(out Movement movement);
            movement.Init(_movementJoystick, _rotationJoystick, _playerData.SmoothMovementTime, _playerData.MovementSpeed);

            controller.GetSafeComponent(out Health health);
            health.Init(_healthBar, _playerData.MaxHealth);

            controller.GetSafeComponent(out Shooter shooter);
            _shooter = shooter;

            controller.GetSafeComponent(out CoinPicker coinPicker);
            coinPicker.Init(this);
        }
        #endregion

        #region Public Methods
        internal void PerformShoot() => _shooter.PerformShoot();

        internal void AddCoin()
        {
            _coinsAmount++;

            _coins.text = _coinsAmount.ToString();
        }
        #endregion
    }
}