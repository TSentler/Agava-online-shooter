using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class ConnectToLobby : MonoBehaviour
    {
        private Coroutine _joinLobbyCoroutine;

        private ConnectionCallbackCatcher _connectCatcher;
        private LobbyCallbackCatcher _lobbyCatcher;
        
        public event UnityAction<string[]> OnRoomNamesUpdate;

        private void Awake()
        {
            _connectCatcher = FindObjectOfType<ConnectionCallbackCatcher>();
            _lobbyCatcher = FindObjectOfType<LobbyCallbackCatcher>();
        }

        private void OnEnable()
        {
            _connectCatcher.OnConnectToMaster += JoinLobby;
            _lobbyCatcher.OnRoomsUpdate += RoomNamesUpdate;
        }

        private void OnDisable()
        {
            _connectCatcher.OnConnectToMaster -= JoinLobby;
            _lobbyCatcher.OnRoomsUpdate -= RoomNamesUpdate;
        }

        private void JoinLobby()
        {
            if (_joinLobbyCoroutine != null)
                return;

            _joinLobbyCoroutine = 
                StartCoroutine(JoinLobbyCoroutine());
        }
        
        private IEnumerator JoinLobbyCoroutine()
        {
            while (PhotonNetwork.InLobby == false)
            {
                Debug.Log("TryJoinLobby");
                PhotonNetwork.JoinLobby();
                yield return new WaitForSeconds(2f);
            }
            _joinLobbyCoroutine = null;
        }

        private void RoomNamesUpdate(List<RoomInfo> roomList)
        {
            var roomNames = (from room in roomList
                where room.IsVisible && room.IsOpen
                orderby room.Name
                select room.Name).ToArray();
            OnRoomNamesUpdate?.Invoke(roomNames);
        }
    }
}
