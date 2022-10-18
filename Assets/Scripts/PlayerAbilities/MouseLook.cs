using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerAbilities
{
    public class MouseLook : MonoBehaviour
    {
        private float _xRotation;
        
        [SerializeField] private float _mouseSensetivity;
        [SerializeField] private Transform _playerBody;
        [SerializeField] private PhotonView _photonView;

        public event UnityAction<float> OnLookChange;
        
        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;

            if(_photonView.IsMine == false)
            {
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (_photonView.IsMine)
            {
                var sensetivityFactor = _mouseSensetivity * Time.deltaTime;
                float mouseX = Input.GetAxis("Mouse X") * sensetivityFactor;
                float mouseY = Input.GetAxis("Mouse Y") * sensetivityFactor;

                _xRotation -= mouseY;
                _xRotation = Mathf.Clamp(_xRotation, -80, 80);

                transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

                _playerBody.Rotate(Vector3.up * mouseX);
                OnLookChange?.Invoke(_xRotation);
            }
        }
    }
}
