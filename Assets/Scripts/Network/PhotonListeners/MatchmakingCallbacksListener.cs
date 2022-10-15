using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.Events;

namespace Network
{
    public class MatchmakingCallbacksListener : IMatchmakingCallbacks
    {
        public event UnityAction<List<FriendInfo>> OnFriendsUpdate;
        public event UnityAction OnCreateRoom, OnJoinRoom, OnRoomLeft;
        public event UnityAction<short, string> OnCreateRoomFail,
            OnJoinRoomFail, OnJoinRandomFail;

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            OnFriendsUpdate?.Invoke(friendList);
        }

        public void OnCreatedRoom()
        {
            OnCreateRoom?.Invoke();
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            OnCreateRoomFail?.Invoke(returnCode, message);
        }

        public void OnJoinedRoom()
        {
            OnJoinRoom?.Invoke();
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            OnJoinRoomFail?.Invoke(returnCode, message);
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            OnJoinRandomFail?.Invoke(returnCode, message);
        }

        public void OnLeftRoom()
        {
            OnRoomLeft?.Invoke();
        }
    }
}
