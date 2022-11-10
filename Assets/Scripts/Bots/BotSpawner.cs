using Photon.Pun;
using UnityEngine;

namespace PlayerAbilities
{
    public class BotSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform[] _spawnPoints;

        private void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            int spawnId = Random.Range(0, _spawnPoints.Length - 1);
            
            _playerPrefab.GetComponent<PlayerInfo>().ToBot();
            PhotonNetwork.Instantiate(_playerPrefab.name,
                _spawnPoints[spawnId].position, Quaternion.identity, 0);
        }
    }
}
