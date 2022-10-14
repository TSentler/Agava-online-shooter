using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PhotonView))]
public class PlayerHand : MonoBehaviour, IPunObservable
{
    [SerializeField] private Camera _camera;
    [SerializeField] private List<Gun> _guns;

    private PhotonView _photonView;
    private Gun _currentGun;

    public event UnityAction<Gun> GunChanged;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _currentGun = _guns[0];
        _currentGun.gameObject.SetActive(true);
        GunChanged?.Invoke(_currentGun);
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _currentGun.Shoot(_camera);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _currentGun.Reload();
            }
        }       
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    public void SetNewGun(Gun newGun)
    {
        _photonView.RPC(nameof(SetNewGunRPC), RpcTarget.All, newGun);
    }

    [PunRPC]
    private void SetNewGunRPC(Gun newGun)
    {
        if (_photonView.IsMine == false)
        {
            return;
        }
        else
        {
            _currentGun.gameObject.SetActive(false);
            _currentGun = newGun;
            _currentGun.gameObject.SetActive(true);
            GunChanged?.Invoke(_currentGun);
        }
    }
}
