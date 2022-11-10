using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponsHolder : MonoBehaviour
{
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private GameObject[] _weaponsInThirdPersons;
    [SerializeField] private Animator _animator;
    [SerializeField] private RuntimeAnimatorController _controllerTwoHand;
    [SerializeField] private RuntimeAnimatorController _controllerOneHand;

    private int _currentGunId = 0;

    public event UnityAction<Transform> GunChanged;

    private void OnEnable()
    {   
        GunChanged?.Invoke(_weapons[0].gameObject.transform);
        _photonView.RPC(nameof(EnableGunInStart), RpcTarget.All);
        _currentGunId = 0;
    }

    public void SetNewGun(int id)
    {
        _photonView.RPC(nameof(EnableNewGun), RpcTarget.All, id);
        _currentGunId = id;
    }

    [PunRPC]
    private void EnableNewGun(int id)
    {
        _weapons[_currentGunId].SetActive(false);
        _weaponsInThirdPersons[_currentGunId].SetActive(false);
        _weapons[id].SetActive(true);
        GunChanged?.Invoke(_weapons[id].gameObject.transform);
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

    [PunRPC]
    private void EnableGunInStart()
    {
        _weapons[_currentGunId].SetActive(false);
        _weapons[0].SetActive(true);
        _weaponsInThirdPersons[_currentGunId].SetActive(false);
        _weaponsInThirdPersons[0].SetActive(true);
        _animator.runtimeAnimatorController = _controllerOneHand;
    }
}
