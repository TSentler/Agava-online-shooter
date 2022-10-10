using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    [RequireComponent(typeof(PhotonView))]
    public class PingSender : MonoBehaviour
    {
        private float _sendPingInterval;
        private PhotonView _photonView;
        
        private float _nextSendPingTime = 0f;

        public event UnityAction<Player, int> OnReceivePing;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
        }

        private void Update()
        {
            SendPing();
        }

        private void SendPing()
        {
            if (Time.unscaledTime < _nextSendPingTime)
                return;
 
            _nextSendPingTime = Time.unscaledTime + _sendPingInterval;
 
            _photonView.RPC(nameof(ReceivePingRPC), RpcTarget.All, 
                PhotonNetwork.LocalPlayer,
                PhotonNetwork.GetPing());
        }
        
        [PunRPC]
        private void ReceivePingRPC(Player player, int ping)
        {
            OnReceivePing?.Invoke(player, ping);
        }
        
        public void Init(float sendPingInterval)
        {
            _sendPingInterval = sendPingInterval;
        }
        
    }
}