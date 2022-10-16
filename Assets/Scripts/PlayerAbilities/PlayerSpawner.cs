using Photon.Pun;
using UnityEngine;

namespace PlayerAbilities
{
    public class PlayerSpawner : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject _playerPrefab;

        private void Start()
        {
            PhotonNetwork.Instantiate(_playerPrefab.name, transform.position,
                Quaternion.identity, 0);
        }
    }
}
