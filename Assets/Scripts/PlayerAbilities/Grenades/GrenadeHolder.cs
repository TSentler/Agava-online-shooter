using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeHolder : MonoBehaviour
{
    [SerializeField] private int _maxGrenades;
    [SerializeField] private Grenade _grenade;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _spawnPoint;

    private int _currentGrenade;

    private void Awake()
    {
        _currentGrenade = _maxGrenades;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
            _currentGrenade--;
        }
    }

    private void ThrowGrenade()
    {
        if (_currentGrenade <= 0)
            return;

       GameObject grenade =  PhotonNetwork.Instantiate(_grenade.name, _spawnPoint.position, Quaternion.identity);
        grenade.GetComponent<Grenade>().Instantiate(_camera);
    }
}
