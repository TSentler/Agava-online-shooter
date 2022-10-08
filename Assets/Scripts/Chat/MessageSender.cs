using System.Collections.Generic;
using Pack;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Chat
{
    [RequireComponent(typeof(PhotonView))]
    public class MessageSender : MonoBehaviour
    {
        private PhotonView _view;
        [SerializeField] private List<ChatMessage> _messages = new();
        
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
            _view.RPC(nameof(SendMessageRPC), RpcTarget.All,
                PhotonNetwork.NickName, text);
        }
        
        [PunRPC]
        private void SendMessageRPC(string nick, string text)
        {
            var message = new ChatMessage(nick, text);
            _messages.Add(message);
            _chatView.AddMessage(nick, text);
        }

        [PunRPC]
        private void SyncMessagesRPC(object container)
        {
            var messages = (List<ChatMessage>)Packer.
                ByteArrayToObject((byte[])container);
            List<ChatMessage> temp = new();
            temp.AddRange(_messages);
            _messages.Clear();
            _messages.AddRange(messages);
            _messages.AddRange(temp);
            _chatView.Clear();
            foreach (var message in _messages)
            {
                _chatView.AddMessage(message.Nick, message.Text);
            }
        }

        public void SyncMessages(Player player)
        {
            if (PhotonNetwork.IsMasterClient == false)
                return;

            byte[] container = Packer.ObjectToByteArray((object)_messages);
            _view.RPC(nameof(SyncMessagesRPC), player, (object)container);
        }
    }
}
