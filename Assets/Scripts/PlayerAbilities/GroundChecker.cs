using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerAbilities
{
    public class GroundChecker : MonoBehaviour
    {
        [SerializeField] private Transform _groundPoint;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private float _groundOverlapRadius = 0.15f;
        
        private bool _isGrounded;
        
        public event UnityAction Grounded, Jumped;
        
        public bool IsGround { get; private set; }

        private void Update()
        {
            GroundCheck();
            if (IsGround)
            {
                GroundedNowCheck();
            }
            else
            {
                JumpedNowCheck();
            }
        }

        private void GroundCheck()
        {
            var hitColliders = new Collider[1];
            hitColliders = Physics.OverlapSphere(_groundPoint.position, 
                _groundOverlapRadius, _groundMask);
            IsGround = hitColliders.Length > 0;
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
