using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace PlayerAbilities
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _cooldown;

        private void Start()
        {
            Spawn();
        }

        public void SpawnPlayer(PlayerHealth player)
        {
            Spawn(player);
            //StartCoroutine(SpawnWithCooldown(player));
        }

        private IEnumerator SpawnWithCooldown(PlayerHealth player)
        {
            yield return new WaitForSeconds(_cooldown);

            Spawn(player);
        }

        private void Spawn(PlayerHealth player)
        {
            int spawnId = Random.Range(0, _spawnPoints.Length - 1);
            Debug.Log((Vector3)Random.insideUnitCircle);
            player.transform.position = _spawnPoints[spawnId].position + new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
            player.EnableObject();
        }

        private void Spawn()
        {
            int spawnId = Random.Range(0, _spawnPoints.Length - 1);
            //_playerPrefab.GetComponent<PlayerInfo>().ToPlayer();
            Debug.Log((Vector3)Random.insideUnitCircle);
            PhotonNetwork.Instantiate(_playerPrefab.name,
                _spawnPoints[spawnId].position + new Vector3(Random.Range(-1,1), 0, Random.Range(-1, 1)), Quaternion.identity, 0);
        }
    }
}
