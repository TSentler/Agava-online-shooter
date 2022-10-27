using System;
using CharacterInput;
using UnityEngine;

namespace PlayerAbilities
{
    public class MovementPresenter : MonoBehaviour
    {
        private readonly int 
            _forwardName = Animator.StringToHash("InputForward"),
            _rightName = Animator.StringToHash("InputRight");

        [SerializeField] private MonoBehaviour _inputSourceBehaviour;
        [SerializeField] private Animator _animator;

        private ICharacterInputSource _inputSource;
        
        private void OnValidate()
        {
            if (_inputSourceBehaviour 
                && !(_inputSourceBehaviour is ICharacterInputSource))
            {
                Debug.LogError(nameof(_inputSourceBehaviour) + " needs to implement " + nameof(ICharacterInputSource));
                _inputSourceBehaviour = null;
            }
        }

        private void Awake()
        {
            _inputSource = (ICharacterInputSource)_inputSourceBehaviour;
        }

        private void Update()
        {
            UpdateDirection(_inputSource.MovementInput);
        }

        private void UpdateDirection(Vector2 direction)
        {
            _animator.SetFloat(_forwardName, direction.y);
            _animator.SetFloat(_rightName, direction.x);
        }
    }
}
