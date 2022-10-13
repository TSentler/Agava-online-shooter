using Network;
using Photon.Realtime;
using UnityEngine;

namespace Chat.Network
{
    [RequireComponent(typeof(MessageSender))]
    public class ChatInitializer : MonoBehaviour
    {
        private InRoomCallbackCatcher _enteredCatcher;
        private MessageSender _sender;
        
        private void Awake()
        {
            _sender = GetComponent<MessageSender>();
            _enteredCatcher = FindObjectOfType<InRoomCallbackCatcher>();
        }

        private void OnEnable()
        {
            _enteredCatcher.OnPlayerEnter += PlayerEnterHandler;
        }

        private void OnDisable()
        {
            _enteredCatcher.OnPlayerEnter -= PlayerEnterHandler;
        }

        private void PlayerEnterHandler(Player player)
        {
            _sender.SyncMessages(player);
        }
    }
}
