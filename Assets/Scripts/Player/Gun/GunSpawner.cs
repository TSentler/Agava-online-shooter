using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))] 
public class GunSpawner : MonoBehaviour
{
    [SerializeField] private List<Gun> _guns;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _timerToSpawn;

    private Gun _newGun;

    private void Start()
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerHand playerHand))
        {
            playerHand.SetNewGun(_newGun);
        }
    }

    private void SetNextGun()
    {

    }
}
