using System;
using System.Collections.Generic;
using Network.UI;
using TMPro;
using UnityEngine;

namespace Network
{
    public class ConnectToServerView : MonoBehaviour
    {
        [SerializeField] private GameObject _waitPanel;
        [SerializeField] private TMP_InputField _create;
        [SerializeField] private TMP_Dropdown _join;
        [SerializeField] private TMP_Text _joinLabel;

        public string CreateRoomName => _create.text;

        public string JoinRoomName =>
            _join.options.Count == 0
                ? String.Empty
                : _join.options[_join.value].text;

        private void Awake()
        {
            _waitPanel.SetActive(true);
            _create.text = "Room1";
            _join.ClearOptions();
        }

        public void SetVersion(string version)
        {
            FindObjectOfType<VersionText>().SetVersion(version);
        }
        
        public void WaitShow()
        {
            _waitPanel.SetActive(true);
        }

        public void WaitHide()
        {
            _waitPanel.SetActive(false);
        }

        public void RoomUpdate(List<string> roomNames)
        {
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
    }
}
