using Photon.Pun;
using UnityEngine;

namespace Player
{
#if UNITY_EDITOR
    [DisallowMultipleComponent, RequireComponent(typeof(PhotonView))]
#endif
    public sealed class Movement : MonoBehaviour
    {
        #region Editor Fields
        [SerializeField] private Transform _model = null;
        #endregion

        #region Fields
        private bool _movementIsLocked;
        private float _smoothTime, _movementSpeed;
        private float _minX, _maxX, _minY, _maxY;
        private Rigidbody2D _rigidbody = null;
        private PhotonView _view = null;
        private Joystick _movementJoystick = null, _rotationJoystick = null;
        private Vector3 _smoothMoveVelocity, _moveAmount;
        #endregion

        #region Properties
        public bool MovementIsLocked { get => _movementIsLocked; set => _movementIsLocked = value; }
        #endregion

        #region MonoBehaviour API
        private void Awake()
        {
            _view = GetComponent<PhotonView>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            if (_view.IsMine)
            {
                float aspect = (float)Screen.width / Screen.height;

                float worldHeight = Camera.main.orthographicSize * 2;
                float worldWidth = worldHeight * aspect;

                _minX = (worldWidth / 2) + 0.5f;
                _maxX = (worldWidth / 2) - 0.5f;
                _minY = (worldHeight / 2) + 0.5f;
                _maxY = (worldHeight / 2) - 0.5f;
            }
        }

        private void Update()
        {
            if (!_view.IsMine) return;
            if (_movementIsLocked) return;

            Look();
            Move();
        }

        private void FixedUpdate()
        {
            if (!_view.IsMine) return;
            if (_movementIsLocked) return;

            _rigidbody.MovePosition(_rigidbody.position + ((Vector2)transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime));
        }
        #endregion

        #region Methods
        private void Look()
        {
            if (_rotationJoystick.Direction == Vector2.zero) return;

            float angle = Mathf.Atan2(_rotationJoystick.Vertical, _rotationJoystick.Horizontal) * Mathf.Rad2Deg;
            _model.rotation = Quaternion.AngleAxis(angle, transform.forward);
        }

        private void Move()
        {
            Vector3 moveDirection = _movementJoystick.Direction;

            _moveAmount = Vector3.SmoothDamp(_moveAmount, moveDirection * _movementSpeed, ref _smoothMoveVelocity, _smoothTime);
        }
        #endregion

        #region Public Methods
        internal void Init(Joystick movementJoystick, Joystick rotationJoystick, float smoothTime, float movementSpeed)
        {
            _movementIsLocked = true;

            _movementJoystick = movementJoystick;
            _rotationJoystick = rotationJoystick;

            _smoothTime = smoothTime;
            _movementSpeed = movementSpeed;
        }
        #endregion
    }
}