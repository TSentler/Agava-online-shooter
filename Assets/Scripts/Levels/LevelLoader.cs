using System;
using Network;
using Photon.Pun;
using UnityEngine;

namespace Levels
{
    public enum LevelNames
    {
        Room1, RoomCorridor, GameMapScene
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
            _matchCallback.OnJoinRoom += JoinRoomHandler;
        }

        private void OnDisable()
        {
            _matchCallback.OnJoinRoom += JoinRoomHandler;
        }

        private void JoinRoomHandler()
        {
            PhotonNetwork.LoadLevel(_levelName.ToString());
        }
    }
}
