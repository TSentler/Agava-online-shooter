using System;
using UnityEngine;

namespace PlayerAbilities.Gun
{
    public class HandPresenter : MonoBehaviour
    {
        private readonly int _isRifleName = 
            Animator.StringToHash("IsRifle");

        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerHand _playerHand;

        private void OnEnable()
        {
            _playerHand.GunChanged += GunChangedHandler;
        }

        private void OnDisable()
        {
            _playerHand.GunChanged -= GunChangedHandler;
        }

        private void GunChangedHandler(int quantity, int max)
        {
            _animator.SetBool(_isRifleName, _playerHand.CurrentGunId == 1);
        }
    }
}