using CharacterInput;
using UnityEngine;

namespace PlayerAbilities
{
    public class MovementPresenter : MonoBehaviour
    {
        private readonly int 
            _forwardName = Animator.StringToHash("InputForward"),
            _rightName = Animator.StringToHash("InputRight");

        private const string ForwardName = "IsRun";
        private const string RightName = "InputRight";
        private const string BackMoveName = "IsBackRun";
        private const string RightMoveName = "IsRight";
        private const string LeftMoveName = "IsLeft";

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
            Debug.Log(direction);
            //_animator.SetFloat(ForwardName, direction.y);
            //_animator.SetFloat(RightName, direction.x);
            if (direction.y > 0)
            {
                _animator.SetBool(ForwardName, true);
            }

            if (direction.y < 0)
            {
                _animator.SetBool(BackMoveName, true);
            }

            if (direction.y == 0)
            {
                _animator.SetBool(BackMoveName, false);
            }

            if (direction.x > 0)
            {
                _animator.SetBool(RightMoveName, true);
            }

            if(direction.x == 0)
            {
                _animator.SetBool(RightMoveName, false);
            }

            if(direction.x < 0)
            {
                _animator.SetBool(LeftMoveName, true);
            }
        }
    }
}
