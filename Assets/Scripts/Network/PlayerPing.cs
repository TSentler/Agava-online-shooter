using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;

namespace Network
{
    public class PlayerPing
    {
        public PlayerPing(Player player, int ping)
        {
            Player = player;
            _pings.Add(ping);
        }
        
        private const int _maximumRecordedPings = 6;
        
        public float LastUpdatedTime { get; private set; } = -1f;
        public readonly Player Player;
        private List<int> _pings = new();

        public int ReturnAveragePing()
        {
            if (_pings.Count < _maximumRecordedPings)
            {
                return -1;
            }
            
            int sum = _pings.Sum();
            return Mathf.CeilToInt(sum / _pings.Count);
        }

        public void AddPing(int value)
        {
            if (_pings.Count >= _maximumRecordedPings)
                _pings.RemoveAt(0);

            _pings.Add(value);
            LastUpdatedTime = Time.unscaledTime;
        }
    }
}