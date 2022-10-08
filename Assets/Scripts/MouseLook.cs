using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _mouseSensetivity;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private PhotonView _phoronViev;

    private float _xRotation;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }

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
            float mouseX = Input.GetAxis("Mouse X") * _mouseSensetivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * _mouseSensetivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90, 90);

            transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

            _playerBody.Rotate(Vector3.up * mouseX);
        }
        
    }
}
