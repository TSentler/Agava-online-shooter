using Photon.Pun;
using System.Collections;
using CharacterInput;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayerAbilities
{
    public class MouseLook : MonoBehaviour
    {      
        private const string MouseSensitivitySaveKey = "MouseSensitivity";

        [SerializeField] private MonoBehaviour _inputSourceBehaviour;
        [SerializeField] private float _mouseSensetivity;
        [SerializeField] private float _shootSensetivity;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _playerBody;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private Transform _weapon;
        [SerializeField] private float _standartSensetivity;

        private ICharacterInputSource _inputSource;
        private float _xRotation;
        private Vector3 _deltaPosition;
        private Slider _slieder;
        private MouseSensitivityChange _mouseSensitivityChange;

        public float XRotation => _xRotation;
        
        public event UnityAction<float> OnLookChange;

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
                Initialize((ICharacterInputSource)_inputSourceBehaviour);
            }
            
            Cursor.lockState = CursorLockMode.Locked;

            if (_photonView.IsMine == false)
            {
                _camera.gameObject.SetActive(false);
            }

            _mouseSensitivityChange = FindObjectOfType<MouseSensitivityChange>();
            _slieder = _mouseSensitivityChange.gameObject.GetComponent<Slider>();

            if (_photonView.IsMine)
            {
                if (PlayerPrefs.HasKey(MouseSensitivitySaveKey))
                {
                    _slieder.value = PlayerPrefs.GetFloat(MouseSensitivitySaveKey);
                }
                else
                {
                    _slieder.value = _standartSensetivity;
                }
            }           
        }

        private void OnEnable()
        {
            _slieder.onValueChanged.AddListener(ChangeSensetivity);
        }

        private void OnDisable()
        {
            _slieder.onValueChanged.RemoveListener(ChangeSensetivity);
        }

        private void Update()
        {
            if (_photonView.IsMine)
            {
                var sensetivityFactor = _mouseSensetivity * Time.deltaTime;
                float mouseX = _inputSource.MouseInput.x * sensetivityFactor;
                float mouseY = _inputSource.MouseInput.y * sensetivityFactor;

                if (mouseX != 0 || mouseY !=0)
                {
                    MouseMove(mouseX, mouseY);
                }                
            }
        }

        public void Shoot(float rifleRecoilXMin,float rifleRecoilYMin,float rifleRecoilXMax, float rifleRecoilYMax, float magnitude, float shootDelay)
        {         
            float mouseX = Random.Range(rifleRecoilXMin, rifleRecoilXMax) /** sensetivityFactor*/;
            float mouseY = Random.Range(rifleRecoilYMin, rifleRecoilYMax) /** sensetivityFactor*/;

            MouseMove(mouseX, mouseY);
        }
        public void MouseMove(float mouseX,float mouseY)
        {
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -80, 80);

            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

            _playerBody.Rotate(Vector3.up * mouseX);
            OnLookChange?.Invoke(_xRotation);
        }

        private void ChangeSensetivity(float value)
        {
            _mouseSensetivity = _slieder.value;
            PlayerPrefs.SetFloat(MouseSensitivitySaveKey, _slieder.value);
        }
    }
}
