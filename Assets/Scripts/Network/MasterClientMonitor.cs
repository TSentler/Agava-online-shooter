//original script https://pastebin.com/QxavvqRt
//https://www.youtube.com/watch?v=yrB7Hyh2BE4&t=381s

using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class PingSender
    {
        public PingSender(float sendPingInterval, PhotonView photonView)
        {
            _sendPingInterval = sendPingInterval;
            _photonView = photonView;
        }
        
        private readonly float _sendPingInterval;
        private readonly PhotonView _photonView;
        
        private float _nextSendPingTime = 0f;

        public event UnityAction<Player, int> OnReceivePing;
        
        public void CheckSendPing()
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
    }
    
    public class MasterClientMonitor : MonoBehaviourPunCallbacks
    {
        private const int _minimumPingDifference = 50;
        private const float _pingCheckInterval = 5f;
        private const float _takeoverRequestTimeout = 3f;
        private const float _sendPingInterval = 5f;
        
        private float _nextCheckChangeMaster = 0f;
        private int _consequtiveHighPingCount = 0;
        private bool _pendingMasterChange = false;
        private float _takeoverRequestTime = -1f;
        private PlayerPingList _playerPings;
        private PingSender _pingSender;

        private void Awake()
        {
            _playerPings = new PlayerPingList(_sendPingInterval);
            _pingSender = new PingSender(_sendPingInterval, photonView);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            _pingSender.OnReceivePing += _playerPings.ReceivePing;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            _pingSender.OnReceivePing -= _playerPings.ReceivePing;
        }

        private void Update()
        {
            _pingSender.CheckSendPing();
            CheckChangeMaster();
            CheckTakeoverTimeout();
        }
 
        
        private void CheckTakeoverTimeout()
        {
            if (_takeoverRequestTime == -1f)
                return;

            var takeoverRequestTimePassed =
                Time.unscaledTime - _takeoverRequestTime;
            if (takeoverRequestTimePassed > _takeoverRequestTimeout)
            {
                _takeoverRequestTime = -1f;
                SetNewMaster(PhotonNetwork.LocalPlayer);
            }
        }
 
        private void SetNewMaster(Player newMaster, bool resetHighPingCount = true)
        {
            if (resetHighPingCount)
                _consequtiveHighPingCount = 0;
            
            PhotonNetwork.SetMasterClient(newMaster);
        }
 
        private void CheckChangeMaster()
        {
            if (!PhotonNetwork.IsConnected 
                || !PhotonNetwork.InRoom || PhotonNetwork.IsMasterClient)
                return;
            
            if (_takeoverRequestTime != -1f)
                return;
            
            if (Time.time < _nextCheckChangeMaster)
                return;
 
            _nextCheckChangeMaster = Time.time + _pingCheckInterval;
 
            _playerPings.RemoveNullPlayers();
 
            if (PhotonNetwork.PlayerList.Length <= 1 
                || _playerPings.CheckMyPingIntervalValid() == false)
                return;

            _playerPings.CalculateLowestAveragePing(
                out var lowestAveragePlayer, out var lowestAveragePing);
 
            if (lowestAveragePlayer == null)
                return;

            var masterPing = _playerPings.GetMasterPing();
            
            if (masterPing == -1)
                return;
            
            if (lowestAveragePlayer != PhotonNetwork.LocalPlayer)
                return;
 
            float masterPingDifference = masterPing - lowestAveragePing;
            if (masterPingDifference > _minimumPingDifference)
                _consequtiveHighPingCount++;
            else
                _consequtiveHighPingCount = 0;
 
            if (_consequtiveHighPingCount >= 3)
            {
                _takeoverRequestTime = Time.unscaledTime;
                photonView.RPC(nameof(RequestMasterClientRPC),
                    RpcTarget.MasterClient, lowestAveragePlayer);
            }
        }


        [PunRPC]
        private void RequestMasterClientRPC(Player requestor)
        {
            if (_pendingMasterChange 
                || PhotonNetwork.IsMasterClient == false)
                return;
 
            _pendingMasterChange = true;
            photonView.RPC(nameof(MasterClientGrantedRPC), requestor);
        }
 
        [PunRPC]
        private void MasterClientGrantedRPC()
        {
            SetNewMaster(PhotonNetwork.LocalPlayer);
        }
 
        private void OnApplicationPause(bool pause)
        {
            if (pause)
                LocallyHandOffMasterClient();
        }
        
        private void OnApplicationFocus(bool focus)
        {
            if (focus == false)
                LocallyHandOffMasterClient();
        }
 
        private void LocallyHandOffMasterClient()
        {
            if (!PhotonNetwork.IsConnected 
                || !PhotonNetwork.InRoom || !PhotonNetwork.IsMasterClient)
                return;
            
            if (PhotonNetwork.PlayerList.Length <= 1)
                return;
 
            _playerPings.RemoveNullPlayers();
            
            _playerPings.CalculateLowestAveragePing(out var lowestPlayer, 
                out var lowestPing, false);

            if (lowestPlayer == null)
                lowestPlayer = _playerPings.GetFirstAnother();
            
            if (lowestPlayer != null)
                SetNewMaster(lowestPlayer);
        }
 
        public override void OnPlayerLeftRoom(Player otherPlayer)
        { 
            _playerPings.Remove(otherPlayer);
        }
 
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            _pendingMasterChange = false;
            _takeoverRequestTime = -1f;
            _consequtiveHighPingCount = 0;
        }
    }
}
