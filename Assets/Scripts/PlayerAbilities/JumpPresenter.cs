using System;
using UnityEngine;

namespace PlayerAbilities
{
    public class JumpPresenter : MonoBehaviour
    {
        private readonly int
            _isGroundName = Animator.StringToHash("IsGround");

        [SerializeField] private Animator _animator;
        [SerializeField] private GroundChecker _groundChecker;

        private void OnEnable()
        {
            _groundChecker.Grounded += OnGround;
            _groundChecker.Jumped += OnGroundChecker;
        }

        private void OnDisable()
        {
            _groundChecker.Grounded -= OnGround;
            _groundChecker.Jumped -= OnGroundChecker;
        }

        private void OnGroundChecker()
        {
            _animator.SetBool(_isGroundName, false);
        }

        private void OnGround()
        {
            _animator.SetBool(_isGroundName, true);
        }
    }
}
