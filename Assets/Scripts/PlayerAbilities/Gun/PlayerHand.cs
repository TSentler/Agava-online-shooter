using Photon.Pun;
using Photon.Realtime;
using PlayerAbilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    public Gun CurentGun => _currentGun;

    public event UnityAction<int, int> UpdateAmmo;
    public event UnityAction ChangeGun;

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

    public override void OnEnable()
    {
        for (int i = 1; i < _guns.Count; i++)
        {
            _guns[i].EmptyAmmo += EquipDefaultGun;
        }
    }

    public override void OnDisable()
    {
        for (int i = 1; i < _guns.Count; i++)
        {
            _guns[i].EmptyAmmo -= EquipDefaultGun;
        }
    }

    private void Start()
    {
        _currentGun = _guns[0];
        _currentGun.gameObject.SetActive(true);
        _currentGun.AmmoCountChange += OnUpdateAmmo;
        //UpdateAmmo?.Invoke(_currentGun.AmmoQuanityGun, _currentGun.MaxAmmoGun);
        InitializePlayersGuns();
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            //if(_currentGun is Rifle)
            //{
            //}
            //else if(_currentGun is Pistol)
            //{
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        _currentGun.Shoot(_camera);
            //    }
            //}


            if (Input.GetKeyDown(KeyCode.R))
            {
                _currentGun.Reload();
            }

            //UpdateAmmo?.Invoke(_currentGun.AmmoQuanityGun, _currentGun.MaxAmmoGun);
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
        _currentGun.AmmoCountChange -= OnUpdateAmmo;
        //_currentGun.gameObject.SetActive(false);      
        _photonView.RPC(nameof(DisableGun), RpcTarget.All);

        foreach (var gun in _guns)
        {
            if (gun.GunID == newGunId)
            {
                _currentGun = gun;
                _currentGun.AmmoCountChange += OnUpdateAmmo;
                ChangeGun?.Invoke();
            }
        }

        _photonView.RPC(nameof(EnableGun), RpcTarget.All);
        //_currentGun.gameObject.SetActive(true);
        //UpdateAmmo?.Invoke(_currentGun.AmmoQuanityGun, _currentGun.MaxAmmoGun);

        if (_photonView.IsMine)
        {
            Hashtable hash = new Hashtable
            {
                { _gunIDName, newGunId }
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }
    }

    public void EquipDefaultGun()
    {
        _photonView.RPC(nameof(EquipDefaultGunRPC), RpcTarget.All);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    [PunRPC]
    private void EquipDefaultGunRPC()
    {
        SetNewGun(0);
    }

    [PunRPC]
    private void EnableGun()
    {
        _currentGun.gameObject.SetActive(true);
    }

    [PunRPC]
    private void DisableGun()
    {
        _currentGun.gameObject.SetActive(false);
    }

    private void OnUpdateAmmo(int ammoQuanityGun, int maxAmmoGun)
    {
        UpdateAmmo?.Invoke(ammoQuanityGun, maxAmmoGun);
    }
}
