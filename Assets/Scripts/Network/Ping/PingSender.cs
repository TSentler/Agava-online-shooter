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
        private const float _sendPingInterval = 5f;
        
        private PhotonView _photonView;
        
        private float _nextSendPingTime = 0f;

        public event UnityAction<Player, int> OnReceivePing;

        public float SendPingInterval => _sendPingInterval;
        
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
 
            SetNextPingTime();
            
            var ping = PhotonNetwork.GetPing();
            if (PhotonNetwork.InRoom)
            {
                _photonView.RPC(nameof(ReceivePingRPC), RpcTarget.All,
                    PhotonNetwork.LocalPlayer,
                    PhotonNetwork.GetPing());
            }
            else
            {
                ReceivePingRPC(PhotonNetwork.LocalPlayer, ping);
            }
        }

        private void SetNextPingTime()
        {
            _nextSendPingTime = Time.unscaledTime + _sendPingInterval;
        }

        [PunRPC]
        private void ReceivePingRPC(Player player, int ping)
        {
            OnReceivePing?.Invoke(player, ping);
        }
        
        public void SendPingImmidiate()
        {
            var ping = PhotonNetwork.GetPing();
            SetNextPingTime();
            ReceivePingRPC(PhotonNetwork.LocalPlayer, ping);
            _photonView.RPC(nameof(ReceivePingRPC), RpcTarget.Others, 
                PhotonNetwork.LocalPlayer, ping);
        }
    }
}