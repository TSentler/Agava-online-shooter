using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class MatchmakingCallbacksCatcher : MonoBehaviour
    {
        private MatchmakingCallbacksListener _matchCallbacks = new();
        
        public event UnityAction<List<FriendInfo>> OnFriendsUpdate
        {
            add => _matchCallbacks.OnFriendsUpdate += value;
            remove => _matchCallbacks.OnFriendsUpdate -= value;
        }
        public event UnityAction OnCreateRoom
        {
            add => _matchCallbacks.OnCreateRoom += value;
            remove => _matchCallbacks.OnCreateRoom -= value;
        }
        public event UnityAction OnJoinRoom
        {
            add => _matchCallbacks.OnJoinRoom += value;
            remove => _matchCallbacks.OnJoinRoom -= value;
        }
        public event UnityAction OnRoomLeft
        {
            add => _matchCallbacks.OnRoomLeft += value;
            remove => _matchCallbacks.OnRoomLeft -= value;
        }
        public event UnityAction<short, string> OnCreateRoomFail
        { 
            add => _matchCallbacks.OnCreateRoomFail += value;
            remove => _matchCallbacks.OnCreateRoomFail -= value;
        }
        public event UnityAction<short, string> OnJoinRoomFail
        { 
            add => _matchCallbacks.OnJoinRoomFail += value;
            remove => _matchCallbacks.OnJoinRoomFail -= value;
        }
        public event UnityAction<short, string> OnJoinRandomFail
        { 
            add => _matchCallbacks.OnJoinRandomFail += value;
            remove => _matchCallbacks.OnJoinRandomFail -= value;
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(_matchCallbacks);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(_matchCallbacks);
        }
    }
}
