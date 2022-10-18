using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

[RequireComponent(typeof(PhotonView))]
public class PlayerHand : MonoBehaviourPunCallbacks, IPunObservable
{
    private readonly string _gunIDName = "GunID";
        
    [SerializeField] private Camera _camera;
    [SerializeField] private List<Gun> _guns;

    private PhotonView _photonView;
    private Gun _currentGun;
    
    public List<Gun> Guns => _guns;
    public int CurrentGunId => _currentGun.GunID;

    public event Action<int, int> GunChanged;

    private void InitializePlayersGuns()
    {
        if (_photonView.IsMine)
            return;
        
        var ownerProps = _photonView.Owner.CustomProperties;
        if (ownerProps.ContainsKey(_gunIDName))
        {
            SetNewGun((int)ownerProps[_gunIDName]);
        }
    }

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        _currentGun = _guns[0];
        _currentGun.gameObject.SetActive(true);
        GunChanged?.Invoke(_currentGun.AmmoQuanity, _currentGun.MaxAmmo);
        InitializePlayersGuns();
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            if (Input.GetMouseButton(0))
            {
                _currentGun.Shoot(_camera);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                _currentGun.Reload();
            }

            GunChanged?.Invoke(_currentGun.AmmoQuanity, _currentGun.MaxAmmo);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        if (changedProps.ContainsKey(_gunIDName) && _photonView.IsMine == false && targetPlayer == _photonView.Owner)
        {
            SetNewGun((int)changedProps[_gunIDName]);
        }
    }

    public void SetNewGun(int newGunId)
    {
        _currentGun.gameObject.SetActive(false);

        foreach (var gun in _guns)
        {
            if (gun.GunID == newGunId)
            {
                _currentGun = gun;
            }
        }

        _currentGun.gameObject.SetActive(true);
        GunChanged?.Invoke(_currentGun.AmmoQuanity, _currentGun.MaxAmmo);

        if (_photonView.IsMine)
        {
            Hashtable hash = new Hashtable
            {
                { _gunIDName, newGunId }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}
