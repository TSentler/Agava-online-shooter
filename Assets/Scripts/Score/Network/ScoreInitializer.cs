using Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Score.Network
{
    [RequireComponent(typeof(ScoreCounter),
        typeof(PhotonView))]
    public class ScoreInitializer : MonoBehaviour
    {
        private PlayerEnteredRoomCatcher _catcher;
        private ScoreCounter _counter;
        private PhotonView _view;
        
        private void Awake()
        {
            _counter = GetComponent<ScoreCounter>();
            _view = GetComponent<PhotonView>();
            _catcher = FindObjectOfType<PlayerEnteredRoomCatcher>();
        }

        private void OnEnable()
        {
            _catcher.OnPlayerEnter += PlayerEnterHandler;
        }

        private void OnDisable()
        {
            _catcher.OnPlayerEnter -= PlayerEnterHandler;
        }

        private void PlayerEnterHandler(Player player)
        {
            _counter.SyncScore(player);
        }
    }
}
