using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class LobbyCallbackCatcher : MonoBehaviour
    {
        private LobbyCallbackListener _lobbyCallbacks;
        
        public event UnityAction OnLobbyJoined
        {
            add => _lobbyCallbacks.OnLobbyJoined += value;
            remove => _lobbyCallbacks.OnLobbyJoined -= value;
        }
        public event UnityAction OnLobbyLeft
        {
            add => _lobbyCallbacks.OnLobbyLeft += value;
            remove => _lobbyCallbacks.OnLobbyLeft -= value;
        }
        public event UnityAction<List<RoomInfo>> OnRoomsUpdate
        { 
            add => _lobbyCallbacks.OnRoomsUpdate += value;
            remove => _lobbyCallbacks.OnRoomsUpdate -= value;
        }
        public event UnityAction<List<TypedLobbyInfo>> OnStatsUpdate
        {
            add => _lobbyCallbacks.OnStatsUpdate += value;
            remove => _lobbyCallbacks.OnStatsUpdate -= value;
        }
        
        private void Awake()
        {
            _lobbyCallbacks = new LobbyCallbackListener();
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(_lobbyCallbacks);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(_lobbyCallbacks);
        }
    }
}
