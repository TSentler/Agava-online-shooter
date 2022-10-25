using Network;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

namespace Levels
{
    public enum LevelNames
    {
        Room1, RoomCorridor, GameMapScene, LargeLevelScene, Room1BotTest
    }
    
    public class LevelLoader : MonoBehaviour
    {
        private LevelNames _levelName;
        private byte _maxPlayersSmallMap = 5;
        private byte _maxPlayersLargeMap = 10;

        private MatchmakingCallbacksCatcher _matchCallback;
        private RoomOptions _smallRoomOptions;
        private RoomOptions _largeRoomOptions;

        private RoomOptions _currentRoomOptions;

        private void Awake()
        {
            _matchCallback = FindObjectOfType<MatchmakingCallbacksCatcher>();
            _smallRoomOptions = new RoomOptions();
            _smallRoomOptions.MaxPlayers = _maxPlayersSmallMap;
            _smallRoomOptions.CleanupCacheOnLeave = true;

            _largeRoomOptions = new RoomOptions();
            _largeRoomOptions.MaxPlayers = _maxPlayersLargeMap;
            _largeRoomOptions.CleanupCacheOnLeave = true;
        }

        private void OnEnable()
        {
            _matchCallback.OnCreateRoom += CreateRoomHandler;
            _matchCallback.OnJoinRandomFail += OnJoinRoomFailed;
        }

        private void OnJoinRoomFailed(short arg0, string arg1)
        {
            CreateRoom(_levelName.ToString());
            CreateRoomHandler();
        }

        private void OnDisable()
        {
            _matchCallback.OnCreateRoom -= CreateRoomHandler;
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
            PhotonNetwork.JoinOrCreateRoom(
                _levelName.ToString(), _smallRoomOptions, TypedLobby.Default);
        }
        
        public void CreateOrJoinBotMap()
        {
            CreateOrJoinMap(LevelNames.Room1BotTest, _smallRoomOptions);
        }

        public void CreateOrJoinSmallMap()
        {
            CreateOrJoinMap(LevelNames.Room1, _smallRoomOptions);
        }

        public void CreateOrJoinLargeMap()
        {
            CreateOrJoinMap(LevelNames.LargeLevelScene, _largeRoomOptions);
        }

        private void CreateOrJoinMap(LevelNames levelName, RoomOptions options)
        {
            _levelName = levelName;
            _currentRoomOptions = options;
            PhotonNetwork.JoinOrCreateRoom(
                _levelName.ToString(), options, TypedLobby.Default);
        }
        
        public void CreateRoom(string name)
        {
            PhotonNetwork.CreateRoom(name, _currentRoomOptions);
        }

        public void JoinRoom(string name)
        {
            PhotonNetwork.JoinRoom(name);
        }
    }
}
