using System;
using Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Levels
{
    public enum LevelNames
    {
        Room1, RoomCorridor, GameMapScene, LargeLevelScene
    }
    
    public class LevelLoader : MonoBehaviour
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
            PhotonNetwork.AutomaticallySyncScene = true;
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
            PhotonNetwork.JoinOrCreateRoom(
                _levelName.ToString(), _roomOptions, null);
        }

        public void CreateRoom(string name)
        {
            PhotonNetwork.CreateRoom(name);
        }

        public void JoinRoom(string name)
        {
            PhotonNetwork.JoinRoom(name);
        }
    }
}
