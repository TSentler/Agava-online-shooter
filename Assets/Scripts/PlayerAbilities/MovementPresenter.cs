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

        private void Update()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            _animator.SetFloat(_forwardName, verticalInput);
            _animator.SetFloat(_rightName, horizontalInput);
        }
    }
}
