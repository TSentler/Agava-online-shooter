using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
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
            Init();
        }

        private void Init()
        {
            _view.RPC(nameof(InitRPC), RpcTarget.All);
        }
        
        [PunRPC]
        private void InitRPC()
        {
            Debug.Log("InitRPC ++");
            if (PhotonNetwork.IsMasterClient == false)
                return;
            
            SyncScore();
        }

        [PunRPC]
        private void SetScoreRPC(int value)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                _score += value;
                SyncScore();
            }
        }

        private void SyncScore()
        {
            _view.RPC(nameof(SyncScoreRPC), RpcTarget.All, _score);
        }

        [PunRPC]
        private void SyncScoreRPC(int value)
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
