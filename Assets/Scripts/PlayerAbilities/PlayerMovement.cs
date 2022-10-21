using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerAbilities
{
    [RequireComponent(typeof(PhotonView),
        typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed = 18f, _gravityFactor = 1f,
            _groundOverlspRadius = 0.1f;
        [SerializeField] private Transform _groundPoint;
        [SerializeField] private LayerMask _groundMask;
        
        private PhotonView _photonView;
        private CharacterController _character;
        private Vector3 _moveDirection;
        private float _ySpeed;
        private bool _isGrounded;
        
        public event UnityAction<Vector3> DirectionChanged;
        public event UnityAction Grounded, Jumped;

        public bool IsGround
        {
            get
            {
                var hitColliders = new Collider[1];
                hitColliders = Physics.OverlapSphere(_groundPoint.position, 
                    _groundOverlspRadius, _groundMask);
                return hitColliders.Length > 0;
            }
        }

        private void Awake()
        {
            _character = GetComponent<CharacterController>();
            _photonView = GetComponent<PhotonView>();
            _photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        private void Update()
        {
            if (_photonView.IsMine == false)
                return;
            
            var inputDirection = GetInputDirection();
            _moveDirection = GetMoveDirection(inputDirection);
            var distance = _moveDirection * _speed;
            distance += Vector3.up * CalculateYSpeed();
            _character.Move(distance * Time.deltaTime);
            _ySpeed = _character.velocity.y;
            DirectionChanged?.Invoke(inputDirection);
        }
        
        private float CalculateYSpeed()
        {
            if (IsGround)
            {
                GroundedNowCheck();
                if (Input.GetButtonDown("Jump"))
                {
                    _ySpeed = _jumpSpeed;
                }
            }
            else
            {
                JumpedNowCheck();
            }
            
            _ySpeed += _gravityFactor * Physics.gravity.y * Time.deltaTime;

            return _ySpeed;
        }


        private Vector3 GetInputDirection()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            return Vector3.right * horizontalInput + 
                   Vector3.forward * verticalInput;
        }
        
        private Vector3 GetMoveDirection(Vector3 inputDirection)
        {
            Vector3 moveDirectionForward = transform.forward * inputDirection.z;
            Vector3 moveDirectionSide = transform.right * inputDirection.x;

            return (moveDirectionForward + moveDirectionSide).normalized;
        }
        
        private void JumpedNowCheck()
        {
            if (_isGrounded)
            {
                _isGrounded = false;
                Jumped?.Invoke();
            }
        }

        private void GroundedNowCheck()
        {
            if (_isGrounded == false)
            {
                _isGrounded = true;
                Grounded?.Invoke();
            }
        }
        
    }
}
