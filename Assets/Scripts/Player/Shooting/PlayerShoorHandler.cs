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
                Debug.Log("Fire");
                if(Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _maxDistance)) 
                {
                    Debug.Log(hit);
                    if (hit.transform.gameObject.TryGetComponent(out PlayerHealth playerHealth))
                    {
                        Debug.Log("1");
                        //if (_photonView.IsMine == false)
                        //{
                            Debug.Log("2");
                            playerHealth.ApplyDamage(_damage);
                        //}
                    }
                }               
            }
        }
    }
}
