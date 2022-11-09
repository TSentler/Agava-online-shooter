using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponsHolder : MonoBehaviour
{
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private PhotonView _photonView;

    private int _currentGunId = 0;

    public event UnityAction<Transform> GunChanged;

    private void OnEnable()
    {
        _weapons[_currentGunId].SetActive(false);
        _weapons[0].SetActive(true);
        GunChanged?.Invoke(_weapons[0].gameObject.transform);
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
        _weapons[id].SetActive(true);
        GunChanged?.Invoke(_weapons[id].gameObject.transform);
    }
}
