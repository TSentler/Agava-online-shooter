using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoorHandler : MonoBehaviour, IPunObservable
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _damage;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _maxDistance))
                {
                    if (hit.transform.gameObject.TryGetComponent(out PlayerHealth playerHealth))
                    {
                        playerHealth.ApplyDamage(_damage);
                    }
                }
            }
        }
    }
}
