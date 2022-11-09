using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(PhotonView))]
public class GunSpawner : MonoBehaviour
{
 
    [SerializeField] private int _id;
    [SerializeField] private GameObject _weapon;
    [SerializeField] private Collider _colider;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private int _cooldown;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out WeaponsHolder weaponsHolder))
        {
            weaponsHolder.SetNewGun(_id);
            _photonView.RPC(nameof(DisableWeapon), RpcTarget.All);
        }
    }

    [PunRPC]
    private void DisableWeapon()
    {
        _colider.enabled = false;
        _weapon.SetActive(false);
        StartCoroutine(EnableWithCooldown());
    }

    [PunRPC]
    private void EnableWeapon()
    {
        _colider.enabled = true;
        _weapon.SetActive(true);
    }

    private IEnumerator EnableWithCooldown()
    {
        yield return new WaitForSeconds(_cooldown);
        _photonView.RPC(nameof(EnableWeapon), RpcTarget.All);
    }
}
