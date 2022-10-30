using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class ConnectToServer : MonoBehaviour
    {
        private Coroutine _connectCoroutine;
        private ConnectionCallbackCatcher _connectCallback;
        
        public event UnityAction OnConnectStart, OnConnectEnd, OnDisconnect;
        
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _connectCallback = FindObjectOfType<ConnectionCallbackCatcher>();
        }

        private void OnEnable()
        {
            _connectCallback.OnConnectToMaster += ConnectedToMasterHandler;
            _connectCallback.OnDisconnnect += DisconnectHandler;
        }

        private void OnDisable()
        {
            _connectCallback.OnConnectToMaster -= ConnectedToMasterHandler;
            _connectCallback.OnDisconnnect -= DisconnectHandler;
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
            OnConnectStart?.Invoke();
            PhotonNetwork.ConnectUsingSettings();
        }

        private void ConnectedToMasterHandler()
        {
            PhotonNetwork.JoinLobby();
            OnConnectEnd?.Invoke();
        }

        private void DisconnectHandler(DisconnectCause cause)
        {
            Debug.Log("Disconnect");
            OnDisconnect?.Invoke();
            Connect();
        }
    }
}
