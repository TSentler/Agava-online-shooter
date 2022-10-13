using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class ConnectionCallbackCatcher : MonoBehaviour
    {
        private ConnectionCallbackListener _connectionCallbacks;
        
        public event UnityAction OnConnect
        {
            add => _connectionCallbacks.OnConnect += value;
            remove => _connectionCallbacks.OnConnect -= value;
        }
        public event UnityAction OnConnectToMaster
        {
            add => _connectionCallbacks.OnConnectToMaster += value;
            remove => _connectionCallbacks.OnConnectToMaster -= value;
        }
        public event UnityAction<DisconnectCause> OnDisconnnect
        { 
            add => _connectionCallbacks.OnDisconnnect += value;
            remove => _connectionCallbacks.OnDisconnnect -= value;
        }
        public event UnityAction<Dictionary<string, object>> OnAuthResponse
        {
            add => _connectionCallbacks.OnAuthResponse += value;
            remove => _connectionCallbacks.OnAuthResponse -= value;
        }
        public event UnityAction<string> OnAuthFail
        {
            add => _connectionCallbacks.OnAuthFail += value;
            remove => _connectionCallbacks.OnAuthFail -= value;
        }
        
        private void Awake()
        {
            _connectionCallbacks = new ConnectionCallbackListener();
        }

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(_connectionCallbacks);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(_connectionCallbacks);
        }
    }
}
