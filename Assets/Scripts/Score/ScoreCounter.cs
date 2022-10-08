using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Score
{
    [RequireComponent(typeof(PhotonView))]
    public class ScoreCounter : MonoBehaviour
    {
        private PhotonView _view;
        private int _score;
    
        [SerializeField] private ScoreCounterText _text;
        
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

        public void SetScore(int value)
        {
            _score += value;
            _view.RPC(nameof(SyncScoreRPC), RpcTarget.All, _score);
        }
    }
}
