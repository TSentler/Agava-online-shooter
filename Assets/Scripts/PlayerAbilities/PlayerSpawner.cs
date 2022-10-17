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

        public void SpawnPlayer(GameObject player)
        {
            StartCoroutine(SpawnWithCooldown(player));
        }

        private IEnumerator SpawnWithCooldown(GameObject player)
        {
            yield return new WaitForSeconds(_cooldown);

            Spawn(player);
        }

        private void Spawn(GameObject player)
        {
            int spawnId = Random.Range(0, _spawnPoints.Length - 1);
            player.transform.position = _spawnPoints[spawnId].position;
            this.player = player;
            _photonView.RPC(nameof(EnablePlayerRPC), RpcTarget.AllBuffered);
        }

        private void Spawn()
        {
            int spawnId = Random.Range(0, _spawnPoints.Length - 1);
            PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoints[spawnId].position, Quaternion.identity, 0);
        }

        [PunRPC]
        private void EnablePlayerRPC()
        {
            player.SetActive(true);
            player = null;
        }
    }
}
