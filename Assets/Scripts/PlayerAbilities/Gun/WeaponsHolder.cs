using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class WeaponsHolder : MonoBehaviour
{
    private readonly string _gunReadyName = "GunReady", 
        _gunIDName = "GunID";
    
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private GameObject[] _weaponsInThirdPersons;
    [SerializeField] private Animator _animator;
    [SerializeField] private RuntimeAnimatorController _controllerTwoHand;
    [SerializeField] private RuntimeAnimatorController _controllerOneHand;

    private int _currentGunId = 0;

    public event UnityAction<Transform> GunChanged;

    private void Awake()
    {
        if (_photonView.Owner.CustomProperties.ContainsKey(_gunIDName))
        {
            if (_photonView.IsMine)
            {
                PhotonNetwork.LocalPlayer.CustomProperties.Remove(_gunIDName);
            }
            else
            {
                _currentGunId = (int)_photonView.Owner.CustomProperties[_gunIDName];
            }
        }
        InitializeImprovements();
    }

    private void OnEnable()
    {   
        SetNewGun(_currentGunId);
    }

    private void OnDisable()
    {
        _currentGunId = 0;
    }

    private void InitializeImprovements()
    {
        if (_photonView.IsMine && PlayerPrefs.HasKey(_gunReadyName))
        {
            _currentGunId = PlayerPrefs.GetInt(_gunReadyName);
            PlayerPrefs.DeleteKey(_gunReadyName);
        }
    }

    public void SetNewGun(int id)
    {
        if (_photonView.IsMine)
        {
            _photonView.RPC(nameof(EnableNewGun), RpcTarget.All, id, 
                PhotonNetwork.LocalPlayer);
        }
        else
        {
            EnableNewGun(id, _photonView.Owner);
        }
    }

    [PunRPC]
    private void EnableNewGun(int id, Player player)
    {
        if (_photonView.IsMine == false)
        {
            HideAllHands();
        }
        
        if (_photonView.Owner == player)
        {
            _currentGunId = id;
            if (_photonView.IsMine)
            {
                GunChanged?.Invoke(_weapons[id].gameObject.transform);
                HideAllHands();
                _weapons[id].SetActive(true);
                SetCustomProperies(id, _gunIDName);
            }
            HideAllWeaponsAtThirdPerson();
            _weaponsInThirdPersons[id].SetActive(true);

            if(id > 0)
            {
                _animator.runtimeAnimatorController = _controllerTwoHand;
            }
            else
            {
                _animator.runtimeAnimatorController = _controllerOneHand;
            }
        }
    }
    
    private void SetCustomProperies(int id, string name)
    {
        var hash = PhotonNetwork.LocalPlayer.CustomProperties;
        if (hash.ContainsKey(name))
        {
            hash[name] = id;
        }
        else
        {
            hash.Add(name, id);
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    private void HideAllWeaponsAtThirdPerson()
    {
        for (int i = 0; i < _weaponsInThirdPersons.Length; i++)
        {
            _weaponsInThirdPersons[i].SetActive(false);
        }
    }
    
    private void HideAllHands()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].SetActive(false);
        }
    }
}
