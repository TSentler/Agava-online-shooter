using System;
using Network;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Levels
{
    public enum LevelNames
    {
        Room1, RoomCorridor
    }
    
    public class LevelLoader : MonoBehaviour
    {
        private MatchmakingCallbacksCatcher _matchCallback;

        [SerializeField] private LevelNames _levelName;
        
        private void Awake()
        {
            _matchCallback = FindObjectOfType<MatchmakingCallbacksCatcher>();
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
                _levelName.ToString(), null, null);
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
