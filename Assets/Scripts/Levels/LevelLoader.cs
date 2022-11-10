using Network;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Levels
{
    public enum LevelNames
    {
        Room1, RoomCorridor, GameMapScene, LargeLevelScene, Room1BotTest, CandyScene_night, SmallMapCity
    }

    public class LevelLoader : MonoBehaviour
    {
        private const string SceneNameKey = "SceneName";

        private LevelNames _levelName;
        private byte _maxPlayersSmallMap = 5;
        private byte _maxPlayersLargeMap = 10;

        private MatchmakingCallbacksCatcher _matchCallback;
        private LobbyCallbackCatcher _lobbyCalback;
        private RoomOptions _botRoomOptions;
        private RoomOptions _smallRoomOptions;
        private RoomOptions _largeRoomOptions;

        private RoomOptions _currentRoomOptions;
        private List<RoomInfo> _roomInfos = new List<RoomInfo>();

        private void Awake()
        {
            _matchCallback = FindObjectOfType<MatchmakingCallbacksCatcher>();
            _lobbyCalback = FindObjectOfType<LobbyCallbackCatcher>();
            _botRoomOptions = new RoomOptions();
            _botRoomOptions.MaxPlayers = _maxPlayersSmallMap;
            _botRoomOptions.CleanupCacheOnLeave = true;
            _botRoomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            _botRoomOptions.CustomRoomPropertiesForLobby = new string[] { SceneNameKey.ToString() };
            _botRoomOptions.CustomRoomProperties.Add(SceneNameKey, LevelNames.Room1BotTest.ToString());

            _smallRoomOptions = new RoomOptions();
            _smallRoomOptions.MaxPlayers = _maxPlayersSmallMap;
            _smallRoomOptions.CleanupCacheOnLeave = true;
            _smallRoomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            _smallRoomOptions.CustomRoomPropertiesForLobby = new string[] { SceneNameKey.ToString() };
            _smallRoomOptions.CustomRoomProperties.Add(SceneNameKey, LevelNames.Room1.ToString());

            _largeRoomOptions = new RoomOptions();
            _largeRoomOptions.MaxPlayers = _maxPlayersLargeMap;
            _largeRoomOptions.CleanupCacheOnLeave = true;
            _largeRoomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            _largeRoomOptions.CustomRoomPropertiesForLobby = new string[] { SceneNameKey.ToString() };
            _largeRoomOptions.CustomRoomProperties.Add(SceneNameKey, LevelNames.SmallMapCity.ToString());
        }

        private void OnEnable()
        {
            _matchCallback.OnCreateRoom += CreateRoomHandler;
            _matchCallback.OnJoinRandomFail += OnJoinRoomFailed;
            _lobbyCalback.OnRoomsUpdate += RoomListUpdate;
        }

        private void OnJoinRoomFailed(short arg0, string arg1)
        {
            CreateRoom(_levelName.ToString());
            CreateRoomHandler();
        }

        private void OnDisable()
        {
            _matchCallback.OnCreateRoom -= CreateRoomHandler;
            _lobbyCalback.OnRoomsUpdate -= RoomListUpdate;
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
            CreateOrJoinMap(LevelNames.Room1BotTest, _botRoomOptions);
        }

        public void CreateOrJoinSmallMap()
        {

            CreateOrJoinMap(LevelNames.Room1, _smallRoomOptions);
        }

        public void CreateOrJoinLargeMap()
        {
            CreateOrJoinMap(LevelNames.SmallMapCity, _largeRoomOptions);
        }

        private void CreateOrJoinMap(LevelNames levelName, RoomOptions options)
        {
            _levelName = levelName;
            _currentRoomOptions = options;
            Debug.Log(options.CustomRoomProperties.ContainsKey(SceneNameKey));
            PhotonNetwork.JoinOrCreateRoom(
                GetOrCreateRoomName(options, levelName), options, TypedLobby.Default);
        }

        private string GetOrCreateRoomName(RoomOptions roomOptions, LevelNames levelName)
        {
            if (_roomInfos.Count != 0)
            {
                for (int i = 0; i < _roomInfos.Count; i++)
                {
                    Debug.Log(_roomInfos[i].CustomProperties.ContainsKey(SceneNameKey) == false);
                    if (_roomInfos[i].CustomProperties.ContainsKey(SceneNameKey) == false 
                        || levelName.ToString() != _roomInfos[i].CustomProperties[SceneNameKey].ToString())
                    {
                        continue;
                    }

                    if (_roomInfos[i].PlayerCount < roomOptions.MaxPlayers)
                    {
                        return _roomInfos[i].Name;
                    }
                }
            }

            return "Room" + Random.Range(0, 1000).ToString() + PhotonNetwork.NickName;
        }

        public void CreateOrJoinTestLargeMap()
        {
            CreateOrJoinMap(LevelNames.SmallMapCity, _largeRoomOptions);
        }

        public void CreateRoom(string name)
        {
            PhotonNetwork.CreateRoom(name, _currentRoomOptions);
        }

        public void JoinRoom(string name)
        {
            PhotonNetwork.JoinRoom(name);
        }

        public void RoomListUpdate(List<RoomInfo> roomInfos)
        {
            _roomInfos.Clear();
            _roomInfos = roomInfos;

            for (int i = 0; i < _roomInfos.Count; i++)
            {
                Debug.Log(roomInfos[i].CustomProperties[SceneNameKey].ToString());
            }
        }
    }
}
