using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace PlayerAbilities
{
    [RequireComponent(typeof(PhotonView),
        typeof(Animator),
        typeof(CharacterController))]
    public class PlayerJump : MonoBehaviour
    {
        private readonly int
            _isAirName = Animator.StringToHash("IsAir");

        [SerializeField] private float _gravity = -0.9f, 
            _jumpSpeed = 5f;
        
        private PhotonView _photonView;
        private Animator _animator;
        private CharacterController _character;
        private float _ySpeed, _stepOffset;

        private bool IsGround => _character.isGrounded;
        private bool CanJump => 
            Mathf.Approximately(_character.velocity.y, 0f) && IsGround;
        
        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _animator = GetComponent<Animator>();
            _character = GetComponent<CharacterController>();
            _stepOffset = _character.stepOffset;
        }

        private void Update()
        {
            if (_photonView.IsMine == false)
                return;
            
            if (IsGround)
            {
                _character.stepOffset = _stepOffset;
                _ySpeed = 0f;
                if (Input.GetButtonDown("Jump"))
                {
                    _ySpeed = _jumpSpeed;
                }
            }
            else
            {
                _character.stepOffset = 0f;
                _ySpeed += _gravity;
            }
            
            Debug.Log(IsGround + " " + _ySpeed + " " + _character.velocity.y);
            _character.Move(Vector3.up * _ySpeed * Time.deltaTime);
        }
    }
}
