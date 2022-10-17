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
        [SerializeField] private PhotonView _photonView;

        private GameObject player;

        private void Start()
        {
            Spawn();
        }

        public void SpawnPlayer(GameObject player, PhotonView photonView)
        {
            StartCoroutine(SpawnWithCooldown(player, photonView));
        }

        private IEnumerator SpawnWithCooldown(GameObject player, PhotonView photonView)
        {
            yield return new WaitForSeconds(_cooldown);

            Spawn(player,photonView);
        }

        private void Spawn(GameObject player, PhotonView photonView)
        {
            int spawnId = Random.Range(0, _spawnPoints.Length - 1);
            player.transform.position = _spawnPoints[spawnId].position;
            this.player = player;
            photonView.RPC("EnablePlayerRPC", RpcTarget.AllBuffered);
        }

        private void Spawn()
        {
            int spawnId = Random.Range(0, _spawnPoints.Length - 1);
            PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoints[spawnId].position, Quaternion.identity, 0);
        }

      
    }
}
