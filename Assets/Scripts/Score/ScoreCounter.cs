using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Score
{
    [RequireComponent(typeof(TMP_Text), 
        typeof(PhotonView))]
    public class ScoreCounter : MonoBehaviour
    {
        private PhotonView _view;
        private TMP_Text _text;
        private int _score;
    
        private void Awake()
        {
            _view = GetComponent<PhotonView>();
            _text = GetComponent<TMP_Text>();
        }
    
        [PunRPC]
        private void SetScoreRPC(int value)
        {
            _score += value;
            _text.text = _score.ToString();
        }
    
        public void SetScore(int value)
        {
            _view.RPC(nameof(SetScoreRPC), RpcTarget.All, value);
        }
    
    }
}
