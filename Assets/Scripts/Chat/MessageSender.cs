using System;
using Photon.Pun;
using UnityEngine;

namespace Chat
{
    [RequireComponent(typeof(PhotonView))]
    public class MessageSender : MonoBehaviour
    {
        private PhotonView _view;
        
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
            _chatView.AddMessage(nick, text);
        }
    }
}
