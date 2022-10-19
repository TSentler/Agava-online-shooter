using System;
using UnityEngine;

namespace PlayerAbilities
{
    public class JumpPresenter : MonoBehaviour
    {
        private readonly int
            _isGroundName = Animator.StringToHash("IsGround");

        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerJump _jump;

        private void OnEnable()
        {
            _jump.Grounded += OnGround;
            _jump.Jumped += OnJump;
        }

        private void OnDisable()
        {
            _jump.Grounded -= OnGround;
            _jump.Jumped -= OnJump;
        }

        private void OnJump()
        {
            _animator.SetBool(_isGroundName, false);
        }

        private void OnGround()
        {
            _animator.SetBool(_isGroundName, true);
        }
    }
}
