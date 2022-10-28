using System;
using PlayerAbilities.AnimationBehaviours;
using UnityEngine;

namespace PlayerAbilities
{
    public class ThrowGrenadePresenter : MonoBehaviour
    {
        [SerializeField] private AimIK _aimIK;
        [SerializeField] private Animator _animator;

        private GrenadeThrowBehaviour _grenadeThrowBehaviour;
        
        private void OnEnable()
        {
            _grenadeThrowBehaviour = _animator
                .GetBehaviour<GrenadeThrowBehaviour>();
            _grenadeThrowBehaviour.Started += OnAnimationStart;
            _grenadeThrowBehaviour.Ended += OnAnimationEnd;
        }

        private void OnDisable()
        {
            _grenadeThrowBehaviour.Started -= OnAnimationStart;
            _grenadeThrowBehaviour.Ended -= OnAnimationEnd;
        }

        private void OnAnimationStart()
        {
            _aimIK.WeightOff();
        }

        private void OnAnimationEnd()
        {
            _aimIK.WeightReset();
        }
    }
}
