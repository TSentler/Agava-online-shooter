using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Score
{
    [RequireComponent(typeof(TMP_Text), 
        typeof(PhotonView))]
    public class ScoreCounter : MonoBehaviour
    {
        private readonly string _scoreName = "Score";
            
        private PhotonView _view;
        private TMP_Text _text;
        private int _score;
    
        private void Awake()
        {
            _view = GetComponent<PhotonView>();
            _text = GetComponent<TMP_Text>();
            if (PhotonNetwork.IsMasterClient 
                && PhotonNetwork.MasterClient.CustomProperties.ContainsKey(
                    _scoreName) == false)
            {
                PhotonNetwork.MasterClient.CustomProperties.Add("Score", _score);
            }
            
        }
    
        [PunRPC]
        private void SetScoreRPC(int value)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _score += value;
                _view.RPC(nameof(SyncScore), RpcTarget.All, _score);
            }
        }

        [PunRPC]
        private void SyncScore(int value)
        {
            _score = value;
            // object score = null; 
            // PhotonNetwork.MasterClient.CustomProperties.TryGetValue("Score", out score);
            Debug.Log("CustomProps " + _score);
            _text.text = _score.ToString();
        }
        
        public void SetScore(int value)
        {
            _view.RPC(nameof(SetScoreRPC), RpcTarget.All, value);
        }
    }
}
