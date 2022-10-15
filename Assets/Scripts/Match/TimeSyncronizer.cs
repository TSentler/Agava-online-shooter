using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

namespace Match
{
    public class TimeSyncronizer : MonoBehaviour
    {
        private readonly string _startTimeName = "StartTime";

        private float _startTime = -1f;

        public float StartTime => _startTime;
        
        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient 
                && PhotonNetwork.CurrentRoom.CustomProperties.
                    ContainsKey(_startTime) == false)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(
                    new Hashtable
                    { { _startTime, PhotonNetwork.Time } });
            }
        }
    }
}
