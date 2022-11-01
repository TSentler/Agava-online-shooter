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
        [SerializeField] private Transform _weapon;
        [SerializeField] private Camera _weaponCamera;

        private float _xRotation;
        private Vector3 _deltaPosition;

        public float XRotation => _xRotation;
        
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
            //var sensetivityFactor = (_shootSensetivity * Time.deltaTime) / PhotonNetwork.GetPing();
            float mouseX = Random.Range(rifleRecoilXMin, rifleRecoilXMax) /** sensetivityFactor*/;
            float mouseY = Random.Range(rifleRecoilYMin, rifleRecoilYMax) /** sensetivityFactor*/;
            //_deltaPosition = new Vector3(mouseX, mouseY);
            //Vector3 target = _weapon.position -= _deltaPosition;
            //_weapon.position += _deltaPosition;
            //StartCoroutine(ReturnToStartPosition(shootDelay, _deltaPosition));
            StartCoroutine(Shake(shootDelay+0.01f, magnitude));
       
            //StartCoroutine(CameraShake(shootDelay + 0.01f, magnitude));

            MouseMove(mouseX, mouseY);
        }

        private IEnumerator ReturnToStartPosition(float shootDelay, Vector3 deltaPosition)
        {
            //_weapon.Translate(target, _weapon.transform);
            yield return new WaitForSeconds(0.05f);
            _weapon.position -= deltaPosition;
        }

        public void MouseMove(float mouseX,float mouseY)
        {
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -80, 80);

            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

            _playerBody.Rotate(Vector3.up * mouseX);
            //_weapon.Rotate(Vector3.up * _xRotation * 2f);
            OnLookChange?.Invoke(_xRotation);
        }

        private IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 orignalPosition = transform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = Random.Range(-0.0002f, 0.0002f) * magnitude;
                float y = Random.Range(0.08f, 0.1f) * magnitude;

                transform.position = new Vector3(x, y, transform.position.z);
              
                elapsed += Time.deltaTime;
                _camera = Camera.main;
                yield return 0;
            }
            transform.position = orignalPosition;
        }
    }
}
