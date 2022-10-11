using Photon.Pun;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        private float _time;
    
        [SerializeField] private float _delay = 1f;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform[] _points;

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient == false
                || PhotonNetwork.CurrentRoom.PlayerCount < 2)
                return;
        
            if (_time <= 0f)
            {
                var point = _points[Random.Range(0, _points.Length)].position;
                PhotonNetwork.Instantiate(_playerPrefab.name, point,
                    Quaternion.identity);
                _time = _delay;
            }

            _time -= Time.deltaTime;
        }
    }
}
