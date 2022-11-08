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

        public void Initialize(ICharacterInputSource inputSource)
        {
            _inputSource = inputSource;
        }
        
        private void Awake()
        {
            if (_inputSource == null)
            {
                _inputSource = (ICharacterInputSource)_inputSourceBehaviour;
            }
        }

        private void Update()
        {
            if (_playerInfo.IsMine == false)
                return;

            UpdateDirection(_inputSource.MovementInput);
        }

        private void UpdateDirection(Vector2 direction)
        {
            _animator.SetFloat(_forwardName, direction.y);
            _animator.SetFloat(_rightName, direction.x);
        }
    }
}
