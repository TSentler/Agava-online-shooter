using System;
using CharacterInput;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerAbilities
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _inputSourceBehaviour;
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed = 18f, 
            _gravityFactor = 1f,
            _groundOverlspRadius = 0.1f;
        [SerializeField] private Transform _groundPoint;
        [SerializeField] private LayerMask _groundMask;
        
        private ICharacterInputSource _inputSource;
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

        private void OnValidate()
        {
            if (_inputSourceBehaviour 
                && !(_inputSourceBehaviour is ICharacterInputSource))
            {
                Debug.LogError(nameof(_inputSourceBehaviour) + " needs to implement " + nameof(ICharacterInputSource));
                _inputSourceBehaviour = null;
            }
        } 
        
        private void Awake()
        {
            _inputSource = (ICharacterInputSource)_inputSourceBehaviour;
            _character = GetComponent<CharacterController>();
        }

        private void Update()
        {
            var inputDirection = new Vector3(_inputSource.MovementInput.x, 
                0f, _inputSource.MovementInput.y);
            var isJump = _inputSource.IsJumpInput;
            Move(inputDirection, isJump);
        }

        private void Move(Vector3 inputDirection, bool isJump)
        {
            _moveDirection = GetMoveDirection(inputDirection);
            var distance = _moveDirection * _speed;
            distance += Vector3.up * CalculateYSpeed(isJump);
            _character.Move(distance * Time.deltaTime);
            _ySpeed = _character.velocity.y;
            DirectionChanged?.Invoke(inputDirection);
        }
        
        private float CalculateYSpeed(bool isJump)
        {
            if (IsGround)
            {
                GroundedNowCheck();
                if (isJump)
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
