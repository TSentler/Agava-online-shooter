using System;
using CharacterInput;
using Photon.Pun;
using UnityEngine;

namespace PlayerAbilities
{
    public class MovementPresenter : MonoBehaviour
    {
        private readonly int 
            _forwardName = Animator.StringToHash("InputForward"),
            _rightName = Animator.StringToHash("InputRight");

        private const string ForwardName = "InputForward";
        private const string RightName = "InputRight";

        [SerializeField] private MonoBehaviour _inputSourceBehaviour;
        [SerializeField] private Animator _animator;
        [SerializeField] private PlayerInfo _playerInfo;

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
            if (_playerInfo.IsMine == false)
                return;

            UpdateDirection(_inputSource.MovementInput);
        }

        private void UpdateDirection(Vector2 direction)
        {
            Debug.Log(direction);
            _animator.SetFloat(ForwardName, direction.y);
            _animator.SetFloat(RightName, direction.x);
        }
    }
}
