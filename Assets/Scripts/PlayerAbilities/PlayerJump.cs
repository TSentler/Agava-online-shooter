using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerAbilities
{
    [RequireComponent(typeof(PhotonView),
        typeof(Animator),
        typeof(CharacterController))]
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] private float _gravity = -0.8f, 
            _jumpSpeed = 15f;
        
        private PhotonView _photonView;
        private CharacterController _character;
        private float _ySpeed, _stepOffset;
        private bool _isGrounded;

        public event UnityAction Grounded, Jumped;
            
        public bool IsGround => _character.isGrounded;
        
        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _character = GetComponent<CharacterController>();
            _stepOffset = _character.stepOffset;
        }

        private void Update()
        {
            if (_photonView.IsMine == false)
                return;
            
            if (IsGround)
            {
                GroundedNowCheck();
                _ySpeed = 0f;
                _character.stepOffset = _stepOffset;
                if (Input.GetButtonDown("Jump"))
                {
                    _ySpeed = _jumpSpeed;
                }
            }
            else
            {
                JumpedNowCheck();
                _character.stepOffset = 0f;
                _ySpeed += _gravity;
            }
            
            _character.Move(Vector3.up * _ySpeed * Time.deltaTime);
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
