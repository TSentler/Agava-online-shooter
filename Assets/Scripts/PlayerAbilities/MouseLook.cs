using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayerAbilities
{
    public class MouseLook : MonoBehaviour
    {      
        [SerializeField] private float _mouseSensetivity;
        [SerializeField] private float _shootSensetivity;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _playerBody;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private Transform _weapon;
        [SerializeField] private Camera _weaponCamera;

        private float _xRotation;
        private Vector3 _deltaPosition;
        private Slider _slieder;
        private MouseSensitivityChange _mouseSensitivityChange;

        public float XRotation => _xRotation;
        
        public event UnityAction<float> OnLookChange;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;

            if (_photonView.IsMine == false)
            {
                _camera.gameObject.SetActive(false);
            }

            _mouseSensitivityChange = FindObjectOfType<MouseSensitivityChange>();
            _slieder = _mouseSensitivityChange.gameObject.GetComponent<Slider>();
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
                float mouseX = Input.GetAxis("Mouse X") * sensetivityFactor;
                float mouseY = Input.GetAxis("Mouse Y") * sensetivityFactor;

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
        }
    }
}
