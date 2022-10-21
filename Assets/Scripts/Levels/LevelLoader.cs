using Network;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    public enum LevelNames
    {
        Room1, RoomCorridor, GameMapScene, LargeLevelScene
    }

    public class LevelLoader : MonoBehaviour, IMatchmakingCallbacks
    {
        [SerializeField] private LevelNames _levelName;
        [SerializeField] private byte _maxPlayersCount;

        private MatchmakingCallbacksCatcher _matchCallback;
        private RoomOptions _roomOptions;

        private void Awake()
        {
            _matchCallback = FindObjectOfType<MatchmakingCallbacksCatcher>();
            _roomOptions = new RoomOptions();
            _roomOptions.MaxPlayers = _maxPlayersCount;
            _roomOptions.CleanupCacheOnLeave = true;
        }

        private void OnEnable()
        {
            _matchCallback.OnCreateRoom += CreateRoomHandler;
        }

        private void OnDisable()
        {
            _matchCallback.OnCreateRoom += CreateRoomHandler;
        }

        private void CreateRoomHandler()
        {
            PhotonNetwork.LoadLevel(_levelName.ToString());
        }

        public void CreateOrJoinRandom()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.JoinRandomOrCreateRoom();
            }
        }

        public void CreateOrJoinByLevelName()
        {
            PhotonNetwork.JoinOrCreateRoom(_levelName.ToString(),_roomOptions,TypedLobby.Default);
        }

        public void CreateRoom(string name)
        {
            PhotonNetwork.CreateRoom(name);
        }

        public void JoinRoom(string name)
        {
            PhotonNetwork.JoinRoom(name);
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
           
        }

        public void OnCreatedRoom()
        {
            Debug.Log("Created");
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            Debug.Log("Create Room failed");
        }

        public void OnJoinedRoom()
        {
            Debug.Log("Join Room");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(_levelName.ToString(), _roomOptions);
            Debug.Log(1);
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(_levelName.ToString(), _roomOptions);
            Debug.Log(2);
        }

        public void OnLeftRoom()
        {
            Debug.Log("Lefr room");
        }
    }
}
