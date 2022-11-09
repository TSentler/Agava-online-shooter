using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsHolder : MonoBehaviour
{
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private PhotonView _photonView;

    private int _currentGineId = 0;

    private void OnEnable()
    {
        _weapons[_currentGineId].SetActive(false);
        _weapons[0].SetActive(true);
    }

    public void SetNewGun(int id)
    {
        _photonView.RPC(nameof(EnableNewGun), RpcTarget.All, id);
        _currentGineId = id;
    }

    [PunRPC]
    private void EnableNewGun(int id)
    {
        _weapons[_currentGineId].SetActive(false);
        _weapons[id].SetActive(true);
    }
}
