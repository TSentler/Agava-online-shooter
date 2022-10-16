using System;
using System.Collections.Generic;
using System.Linq;
using Network.UI;
using TMPro;
using UnityEngine;

namespace Network
{
    public class ConnectToServerView : MonoBehaviour
    {
        private ConnectToServer _connectToServer;
        
        [SerializeField] private GameObject _waitPanel;
        [SerializeField] private TMP_InputField _create;
        [SerializeField] private TMP_Dropdown _join;
        [SerializeField] private TMP_Text _joinLabel;
        [SerializeField] private ConnectToLobby _connectToLobby;

        public string CreateRoomName => _create.text;

        public string JoinRoomName =>
            _join.options.Count == 0
                ? String.Empty
                : _join.options[_join.value].text;

        private void Awake()
        {
            _connectToServer = FindObjectOfType<ConnectToServer>();
            _waitPanel.SetActive(true);
            _create.text = "Room1";
            _join.ClearOptions();
        }

        private void OnEnable()
        {
            _connectToServer.OnConnectStart += WaitShow;
            _connectToServer.OnConnectEnd += WaitHide;
            _connectToLobby.OnRoomNamesUpdate += RoomUpdate;
        }

        private void OnDisable()
        {
            _connectToServer.OnConnectStart -= WaitShow;
            _connectToServer.OnConnectEnd -= WaitHide;
            _connectToLobby.OnRoomNamesUpdate -= RoomUpdate;
        }

        private void WaitShow()
        {
            _waitPanel.SetActive(true);
        }

        private void WaitHide()
        {
            _waitPanel.SetActive(false);
        }

        private void RoomUpdate(string[] roomNames)
        {
            _join.ClearOptions();
            if (roomNames.Length == 0)
            {
                _joinLabel.SetText("Search Rooms..");
            }
            else
            {
                _join.AddOptions(roomNames.ToList());
            }
        }
    }
}
