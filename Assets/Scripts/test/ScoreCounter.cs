using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Score
{
    [RequireComponent(typeof(PhotonView))]
    public class ScoreCounter : MonoBehaviour
    {
        private PhotonView _view;
        private int _score;

        [SerializeField] private ScoreCounterText _text;

        public int Score => _score;
        
        private void Awake()
        {
            _view = GetComponent<PhotonView>();
        }

        [PunRPC]
        private void SyncScoreRPC(int score)
        {
            if (PhotonNetwork.IsMasterClient)
                return;

            _score = score;
            _text.SetScore(score);
        }

        public void SyncScore(Player player)
        {
            if (PhotonNetwork.IsMasterClient == false)
                return;

            _view.RPC(nameof(SyncScoreRPC), player, _score);
        }
        
        public void SetScore(int value)
        {
            if (PhotonNetwork.IsMasterClient == false)
                return;
            
            _score += value;
            _text.SetScore(_score);
            _view.RPC(nameof(SyncScoreRPC), RpcTarget.All, _score);
        }
    }
}
