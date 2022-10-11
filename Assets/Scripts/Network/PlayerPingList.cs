using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Network
{
    public class PlayerPingList
    {
        public PlayerPingList(float sendPingInterval)
        {
            _sendPingInterval = sendPingInterval;
        }
        
        private readonly float _sendPingInterval;
        
        private List<PlayerPing> _playerPings = new();

        public void Remove(Player player)
        {
            int index = _playerPings.FindIndex(x => x.Player == player);
            if (index != -1)
                _playerPings.RemoveAt(index);
        }
        
        public void RemoveNullPlayers()
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
        
        public void ReceivePing(Player player, int ping)
        {
            int index = _playerPings.FindIndex(x => x.Player == player);
            if (index == -1)
                _playerPings.Add(new PlayerPing(player, ping));
            else
                _playerPings[index].AddPing(ping);
        }
        
        public bool CheckMyPingIntervalValid()
        {
            var player = PhotonNetwork.LocalPlayer;
            int pingIndex = _playerPings.
                FindIndex(x => x.Player == player);

            if (pingIndex == -1)
                return false;
            
            var lastPingInterval =
                Time.unscaledTime - _playerPings[pingIndex].LastUpdatedTime;
            return lastPingInterval < _sendPingInterval * 2;

        }

        public int GetMasterPing()
        {
            var masterPing = -1;
            var player = PhotonNetwork.MasterClient;
            var masterIndex = _playerPings.
                FindIndex(x => x.Player == player);
            
            if (masterIndex == -1)
                return -1;
            
            var playerPing = _playerPings[masterIndex];
            int averagePing = playerPing.ReturnAveragePing();
            if (averagePing == -1)
                return -1;
            
            var lastPingInterval =
                Time.unscaledTime - playerPing.LastUpdatedTime;
            if (lastPingInterval >= _sendPingInterval * 2)
                return 999999999;
            
            return averagePing;
        }
        
        public void CalculateLowestAveragePing(out Player lowestAveragePlayer,
            out int lowestAveragePing, bool isIncludeLocal = true)
        {
            lowestAveragePing = -1;
            lowestAveragePlayer = null;
            
            for (int pingIndex = 0; pingIndex < _playerPings.Count; pingIndex++)
            {
                var playerPing = _playerPings[pingIndex];
                if (isIncludeLocal == false 
                    && playerPing.Player == PhotonNetwork.LocalPlayer)
                    continue;
                
                int averagePing = playerPing.ReturnAveragePing();
                if (averagePing == -1)
                    continue;
                
                if (averagePing < lowestAveragePing
                    || lowestAveragePlayer == null)
                {
                    lowestAveragePing = averagePing;
                    lowestAveragePlayer = playerPing.Player;
                }
            }
        }

        public Player GetFirstAnother()
        {
            return _playerPings.FirstOrDefault(x =>
                x.Player != PhotonNetwork.LocalPlayer)?.Player;
        }
    }
}