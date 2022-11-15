using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class WeaponsHolder : MonoBehaviour
{
    private readonly string _gunReadyName = "GunReady";
    
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
        _photonView.RPC(nameof(EnableNewGun), RpcTarget.AllBuffered, id, 
            PhotonNetwork.LocalPlayer);
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
