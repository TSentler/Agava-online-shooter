using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        private Coroutine _connectCoroutine;

        [SerializeField] private string _version;
        [SerializeField] private ConnectToServerView _connectView;
        
        private void Start()
        {
            SetVersion();
            Connect();
        }

        private void SetVersion()
        {
            PhotonNetwork.GameVersion = _version;
            _connectView.SetVersion(_version);
        }

        private void Connect()
        {
            if (PhotonNetwork.IsConnectedAndReady)
                return;

            Debug.Log("TryConnect");
            _connectView.WaitShow();
            PhotonNetwork.ConnectUsingSettings();
        }
        
        public void CreateRoom()
        {
            if (_connectView.CreateRoomName == string.Empty)
                return;
            
            PhotonNetwork.CreateRoom(_connectView.CreateRoomName);
        }

        public void JoinRoom()
        {
            if (_connectView.JoinRoomName == string.Empty)
                return;
            
            PhotonNetwork.JoinRoom(_connectView.JoinRoomName);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Game");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            var roomNames = (from room in roomList
                where room.IsVisible && room.IsOpen
                orderby room.Name
                select room.Name).ToList();
            _connectView.RoomUpdate(roomNames);
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("IsConnectAndReady" 
                      + PhotonNetwork.IsConnectedAndReady);
            _connectView.WaitHide();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnect");
            Connect();
        }
    }
}
