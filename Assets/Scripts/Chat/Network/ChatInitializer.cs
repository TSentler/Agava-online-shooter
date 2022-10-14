using Network;
using Photon.Realtime;
using UnityEngine;

namespace Chat.Network
{
    [RequireComponent(typeof(MessageSender))]
    public class ChatInitializer : MonoBehaviour
    {
        private PlayerEnteredRoomCatcher _catcher;
        private MessageSender _sender;
        
        private void Awake()
        {
            _sender = GetComponent<MessageSender>();
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
            _sender.SyncMessages(player);
        }
    }
}
