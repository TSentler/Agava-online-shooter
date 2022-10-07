using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Network
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        private Coroutine _connectCoroutine;
        
        [SerializeField] private GameObject _witPanel;
        [SerializeField] private TMP_InputField _create;
        [SerializeField] private TMP_Dropdown _join;
        [SerializeField] private TMP_Text _joinLabel;

        private void Awake()
        {
            _witPanel.SetActive(true);
            _create.text = "Room1";
            _join.ClearOptions();
        }

        private void Start()
        {
            Connect();
        }

        private void Connect()
        {
            if (PhotonNetwork.IsConnectedAndReady)
                return;

            Debug.Log("TryConnect");
            _witPanel.SetActive(true);
            PhotonNetwork.ConnectUsingSettings();
        }
        
        public void CreateRoom()
        {
            PhotonNetwork.CreateRoom(_create.text);
        }

        public void JoinRoom()
        {
            if (_join.options.Count == 0)
                return;
            
            PhotonNetwork.JoinRoom(_join.options[_join.value].text);
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
            _join.ClearOptions();
            if (roomNames.Count == 0)
            {
                _joinLabel.SetText("Search Rooms..");
            }
            else
            {
                _join.AddOptions(roomNames);
            }
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("IsConnectAndReady" 
                      + PhotonNetwork.IsConnectedAndReady);
            _witPanel.SetActive(false);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnect");
            Connect();
        }
    }
}
