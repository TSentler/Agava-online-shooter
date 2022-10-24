using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerAbilities
{
    public class MouseLook : MonoBehaviour
    {      
        [SerializeField] private float _mouseSensetivity;
        [SerializeField] private float _shootSensetivity;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _playerBody;
        [SerializeField] private PhotonView _photonView;

        private float _xRotation;

        public event UnityAction<float> OnLookChange;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;

            if (_photonView.IsMine == false)
            {
                _camera.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (_photonView.IsMine)
            {
                var sensetivityFactor = _mouseSensetivity / Time.deltaTime / PhotonNetwork.GetPing();
                float mouseX = Input.GetAxis("Mouse X") * sensetivityFactor;
                float mouseY = Input.GetAxis("Mouse Y") * sensetivityFactor;

                if (mouseX != 0 || mouseY !=0)
                {
                    MouseMove(mouseX, mouseY);
                }                
            }
        }

        public void Shoot(float rifleRecoilXMin,float rifleRecoilYMin,float rifleRecoilXMax, float rifleRecoilYMax)
        {
            var sensetivityFactor = _shootSensetivity / Time.deltaTime / PhotonNetwork.GetPing();
            float mouseX = Random.Range(rifleRecoilXMin, rifleRecoilXMax) * sensetivityFactor;
            float mouseY = Random.Range(rifleRecoilYMin, rifleRecoilYMax) * sensetivityFactor;
        
            MouseMove(mouseX, mouseY);
        }

        private void MouseMove(float mouseX,float mouseY)
        {
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -80, 80);

            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

            _playerBody.Rotate(Vector3.up * mouseX);
            OnLookChange?.Invoke(_xRotation);
        }
    }
}
