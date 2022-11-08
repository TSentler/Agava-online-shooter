using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : ItemPresenter
{
    [SerializeField] private List<Gun> _guns;

    private Gun _newGun;

    private void Start()
    {
        _newGun = _guns[GetRandomIndex()];
        _newGun.gameObject.SetActive(true);      
    } 

    private void OnTriggerEnter(Collider other)
    {
        if (CanUse)
        {
            if (other.TryGetComponent(out PlayerHand playerHand))
            {
                playerHand.SetNewGun(_newGun.GunID);
                PhotonView.RPC(nameof(DisableWeapon), RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void DisableWeapon()
    {
        _newGun.gameObject.SetActive(false);
        CanUse = false;
        Collider.enabled = false;
        StartCoroutine(CountdownToSpawn());
    }

    private IEnumerator CountdownToSpawn()
    {
        yield return new WaitForSeconds(TimerToSpawn);
        _newGun = _guns[GetRandomIndex()];
        _newGun.gameObject.SetActive(true);
        Collider.enabled = true;
        CanUse = true;
    }

    private int GetRandomIndex()
    {
        return Random.Range(0, _guns.Count);
    }
}
