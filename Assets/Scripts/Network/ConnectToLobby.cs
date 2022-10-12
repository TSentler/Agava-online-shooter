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
    public class ConnectToLobby : MonoBehaviourPunCallbacks
    {
        private Coroutine _joinLobbyCoroutine;

        public event UnityAction<string[]> OnRoomNamesUpdate;
        
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

        public override void OnConnectedToMaster()
        {
            JoinLobby();
        }

        public override void OnLeftLobby()
        {
            Debug.Log("left lobby");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            var roomNames = (from room in roomList
                where room.IsVisible && room.IsOpen
                orderby room.Name
                select room.Name).ToArray();
            OnRoomNamesUpdate?.Invoke(roomNames);
        }
    }
}
