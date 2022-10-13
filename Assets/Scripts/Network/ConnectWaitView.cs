using System;
using System.Collections.Generic;
using System.Linq;
using Network.UI;
using TMPro;
using UnityEngine;

namespace Network
{
    public class ConnectWaitView : MonoBehaviour
    {
        private ConnectToServer _connectToServer;
        
        [SerializeField] private GameObject _waitPanel;

        private void Awake()
        {
            _connectToServer = FindObjectOfType<ConnectToServer>();
            _waitPanel.SetActive(true);
        }

        private void OnEnable()
        {
            _connectToServer.OnConnectStart += WaitShow;
            _connectToServer.OnConnectEnd += WaitHide;
        }

        private void OnDisable()
        {
            _connectToServer.OnConnectStart -= WaitShow;
            _connectToServer.OnConnectEnd -= WaitHide;
        }

        private void WaitShow()
        {
            _waitPanel.SetActive(true);
        }

        private void WaitHide()
        {
            _waitPanel.SetActive(false);
        }
    }
}
