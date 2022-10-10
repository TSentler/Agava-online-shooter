//original script https://pastebin.com/QxavvqRt
//https://www.youtube.com/watch?v=yrB7Hyh2BE4&t=381s

using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Network
{
    public class MasterClientMonitor : MonoBehaviourPunCallbacks
    {
        private const int _highPingTurnoverRequirement = 3;
        private const int _minimumPingDifference = 50;
        private const float _pingCheckInterval = 5f;
        private const float _takeoverRequestTimeout = 3f;
        private const float _sendPingInterval = 5f;
        
        private List<PlayerPing> _playerPings = new();
        private float _nextCheckChangeMaster = 0f;
        private int _consequtiveHighPingCount = 0;
        private bool _pendingMasterChange = false;
        private float _takeoverRequestTime = -1f;
        private float _nextSendPingTime = 0f;
 
        private void Update()
        {
            CheckSendPing();
            CheckChangeMaster();
            CheckTakeoverTimeout();
        }
 
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            int index = _playerPings.FindIndex(x => x.Player == otherPlayer);
            if (index != -1)
                _playerPings.RemoveAt(index);
        }
 
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            _pendingMasterChange = false;
            _takeoverRequestTime = -1f;
            _consequtiveHighPingCount = 0;
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
 
            RemoveNullPlayers();
 
            Player[] players = PhotonNetwork.PlayerList;
            if (players.Length <= 1)
                return;
 
            int lowestAverageIndex = -1;
            int lowestAveragePing = -1;
            int masterPing = -1;
            int masterIndex = -1;
 
            foreach (Player player in players)
            {
                int pingsIndex = _playerPings.
                    FindIndex(x => x.Player == player);
                if (pingsIndex == -1)
                    continue;

                var playerPing = _playerPings[pingsIndex];
                var lastUpdatedPingTimePassed = 
                    Time.unscaledTime - playerPing.LastUpdatedTime;
                if (player == PhotonNetwork.LocalPlayer)
                {
                    if (lastUpdatedPingTimePassed >= _sendPingInterval * 2)
                        return;
                }
 
                int averagePing = playerPing.ReturnAveragePing();
                if (averagePing != -1)
                {
                    if (averagePing < lowestAveragePing 
                        || lowestAverageIndex == -1)
                    {
                        lowestAveragePing = averagePing;
                        lowestAverageIndex = pingsIndex;
                    }
                    
                    if (playerPing.Player.IsMasterClient)
                    {
                        masterIndex = pingsIndex;
 
                        if (lastUpdatedPingTimePassed >= _sendPingInterval * 2)
                            masterPing = 999999999;
                        else
                            masterPing = averagePing;
                    }
                }
            }
 
            if (lowestAverageIndex == -1)
                return;
            
            if (masterIndex == -1)
                return;
            
            if (_playerPings[lowestAverageIndex].Player != PhotonNetwork.LocalPlayer)
                return;
 
            float masterPingDifference = masterPing - lowestAveragePing;
            if (masterPingDifference > _minimumPingDifference)
                _consequtiveHighPingCount++;
            else
                _consequtiveHighPingCount = 0;
 
            if (_consequtiveHighPingCount >= 3)
            {
                _takeoverRequestTime = Time.unscaledTime;
                photonView.RPC(nameof(RPC_RequestMasterClient),
                    RpcTarget.MasterClient, _playerPings[lowestAverageIndex].Player);
            }
        }
 
        private void CheckSendPing()
        {
            if (Time.unscaledTime < _nextSendPingTime)
                return;
 
            _nextSendPingTime = Time.unscaledTime + _sendPingInterval;
 
            photonView.RPC(nameof(RPC_ReceivePing), RpcTarget.All, 
                PhotonNetwork.GetPing());
        }
 
        [PunRPC]
        private void RPC_ReceivePing(Player player, int ping)
        {
            int index = _playerPings.FindIndex(x => x.Player == player);
            if (index == -1)
                _playerPings.Add(new PlayerPing(player, ping));
            else
                _playerPings[index].AddPing(ping);
        }
 
        private void RemoveNullPlayers()
        {
            for (int i = 0; i < _playerPings.Count; i++)
            {
                if (_playerPings[i].Player == null)
                {
                    _playerPings.RemoveAt(i);
                    i--;
                }
            }
        }
 
        [PunRPC]
        private void RPC_RequestMasterClient(Player requestor)
        {
            if (_pendingMasterChange)
                return;
            
            if (!PhotonNetwork.IsMasterClient)
                return;
 
            _pendingMasterChange = true;
            photonView.RPC(nameof(RPC_MasterClientGranted), requestor);
        }
 
        [PunRPC]
        private void RPC_MasterClientGranted()
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
            if (!focus)
                LocallyHandOffMasterClient();
        }
 
        private void LocallyHandOffMasterClient()
        {
            if (!PhotonNetwork.IsConnected 
                || !PhotonNetwork.InRoom || !PhotonNetwork.IsMasterClient)
                return;
            
            if (PhotonNetwork.PlayerList.Length <= 1)
                return;
 
            int otherIndex = -1;
            int lowestIndex = -1;
            int lowestPing = -1;
            for (int i = 0; i < _playerPings.Count; i++)
            {
                if (_playerPings[i].Player == PhotonNetwork.LocalPlayer)
                    continue;
 
                otherIndex = i;
 
                int average = _playerPings[i].ReturnAveragePing();
                if (average < lowestPing || lowestIndex == -1)
                {
                    lowestIndex = i;
                    lowestPing = average;
                }
            }
 
            if (lowestIndex != -1)
            {
                SetNewMaster(_playerPings[lowestIndex].Player);
            }
            else
            {
                if (otherIndex != -1)
                    SetNewMaster(_playerPings[otherIndex].Player);
            }
        }
    }
}
