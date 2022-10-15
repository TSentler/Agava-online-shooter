using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.Events;

namespace Network
{
    public class LobbyCallbackListener : ILobbyCallbacks
    {
        public event UnityAction OnLobbyJoined, OnLobbyLeft;
        public event UnityAction<List<RoomInfo>> OnRoomsUpdate;
        public event UnityAction<List<TypedLobbyInfo>> OnStatsUpdate;

        public void OnJoinedLobby()
        {
            OnLobbyJoined?.Invoke();
        }

        public void OnLeftLobby()
        {
            OnLobbyLeft?.Invoke();
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            OnRoomsUpdate?.Invoke(roomList);
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
            OnStatsUpdate?.Invoke(lobbyStatistics);
        }
    }
}
