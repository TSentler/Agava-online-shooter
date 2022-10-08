using System.Collections.Generic;
using Network.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Chat
{
    [RequireComponent(typeof(PhotonView))]
    public class MessageSender : MonoBehaviourPunCallbacks
    {
        private PhotonView _view;
        private List<ChatMessage> _messages;
        
        [SerializeField] private ChatView _chatView;
        
        private void Awake()
        {
            _view = GetComponent<PhotonView>();
        }

        private void OnEnable()
        {
            _chatView.OnSubmit += SendMesage;
        }

        private void OnDisable()
        {
            _chatView.OnSubmit -= SendMesage;
        }

        private void SendMesage(string text)
        {
            if (_chatView.IsSendReady )
            {
                _view.RPC(nameof(SendMessageRPC), RpcTarget.All,
                    PhotonNetwork.NickName, text);
            }
        }
        
        [PunRPC]
        private void SendMessageRPC(string nick, string text)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var message = new ChatMessage(nick, text);
                _messages.Add(message);
            }
            _chatView.AddMessage(nick, text);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient == false)
                return;
            
            
        }
    }
}
