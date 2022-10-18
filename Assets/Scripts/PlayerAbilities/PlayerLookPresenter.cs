using System;
using UnityEngine;

namespace PlayerAbilities
{
    public class PlayerLookPresenter : MonoBehaviour
    {
        private readonly int _aimName = Animator.StringToHash("Aim");
        
        [SerializeField] private Animator _animator;
        [SerializeField] private MouseLook _mouseLook;

        private void OnEnable()
        {
            _mouseLook.OnLookChange += LookChangeHandler;
        }

        private void OnDisable()
        {
            _mouseLook.OnLookChange -= LookChangeHandler;
        }

        private void LookChangeHandler(float angle)
        {
            angle *= -1;
            _animator.SetFloat(_aimName, angle);
        }
    }
}
