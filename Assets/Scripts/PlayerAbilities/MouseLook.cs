using Photon.Pun;
using UnityEngine;

namespace PlayerAbilities
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private float _mouseSensetivity;
        [SerializeField] private Transform _playerBody;
        [SerializeField] private PhotonView _phoronViev;

        private float _xRotation;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;

            if(_phoronViev.IsMine == false)
            {
                gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (_phoronViev.IsMine)
            {
                var sensetivityFactor = _mouseSensetivity * Time.deltaTime;
                float mouseX = Input.GetAxis("Mouse X") * sensetivityFactor;
                float mouseY = Input.GetAxis("Mouse Y") * sensetivityFactor;

                _xRotation -= mouseY;
                _xRotation = Mathf.Clamp(_xRotation, -90, 90);

                transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

                _playerBody.Rotate(Vector3.up * mouseX);
            }
        }
    }
}
