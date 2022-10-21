using System;
using UnityEngine;

namespace PlayerAbilities
{
    public class JumpPresenter : MonoBehaviour
    {
        private readonly int
            _isGroundName = Animator.StringToHash("IsGround");

        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerMovement _movement;

        private void OnEnable()
        {
            _movement.Grounded += OnGround;
            _movement.Jumped += OnMovement;
        }

        private void OnDisable()
        {
            _movement.Grounded -= OnGround;
            _movement.Jumped -= OnMovement;
        }

        private void OnMovement()
        {
            _animator.SetBool(_isGroundName, false);
        }

        private void OnGround()
        {
            _animator.SetBool(_isGroundName, true);
        }
    }
}
