using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace PlayerAbilities
{
    public class PlayerSpawner : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _cooldown;

        private void Start()
        {
            Spawn();
        }

        public void SpawnPlayer()
        {
            StartCoroutine(SpawnWithCooldown());
        }

        private IEnumerator SpawnWithCooldown()
        {
            yield return new WaitForSeconds(_cooldown);

            Spawn();
        }

        private void Spawn()
        {
            int spawnId = Random.Range(0, _spawnPoints.Length - 1);
            PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoints[spawnId].position, Quaternion.identity, 0);
        }
    }
}
