using System;
using UnityEngine;

namespace PlayerAbilities
{
    public class MovementPresenter : MonoBehaviour
    {
        private readonly int 
            _forwardName = Animator.StringToHash("InputForward"),
            _rightName = Animator.StringToHash("InputRight");

        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerMovement _movement;

        private void OnEnable()
        {
            _movement.DirectionChanged += OnDirectionChange;
        }

        private void OnDisable()
        {
            _movement.DirectionChanged -= OnDirectionChange;
        }

        private void OnDirectionChange(Vector3 direction)
        {
            _animator.SetFloat(_forwardName, direction.z);
            _animator.SetFloat(_rightName, direction.x);
        }
    }
}
