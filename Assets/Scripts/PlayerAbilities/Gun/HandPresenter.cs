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
            _playerHand.UpdateAmmo += GunChangedHandler;
        }

        private void OnDisable()
        {
            _playerHand.UpdateAmmo -= GunChangedHandler;
        }

        private void GunChangedHandler(int quantity, int max)
        {
            var isRifle = _playerHand.CurrentGunId > 0;
            _animator.SetBool(_isRifleName, isRifle);

        }
    }
}
