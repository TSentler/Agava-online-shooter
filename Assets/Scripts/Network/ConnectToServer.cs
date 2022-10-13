using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class ConnectToServer : MonoBehaviour
    {
        private readonly string _defaultSceneName = "MoveTemp";
        
        private Coroutine _connectCoroutine;
        private ConnectionCallbackCatcher _connectCallback;
        private MatchmakingCallbacksCatcher _matchCallback;
        
        public event UnityAction OnConnectStart, OnConnectEnd, OnDisconnect;
        
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            _connectCallback = FindObjectOfType<ConnectionCallbackCatcher>();
            _matchCallback = FindObjectOfType<MatchmakingCallbacksCatcher>();
        }

        private void OnEnable()
        {
            _connectCallback.OnConnectToMaster += ConnectedToMasterHandler;
            _connectCallback.OnDisconnnect += DisconnectHandler;
            _matchCallback.OnJoinRoom += JoinRoomHandler;
        }

        private void OnDisable()
        {
            _connectCallback.OnConnectToMaster -= ConnectedToMasterHandler;
            _connectCallback.OnDisconnnect -= DisconnectHandler;
            _matchCallback.OnJoinRoom -= JoinRoomHandler;
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
            OnConnectEnd?.Invoke();
        }

        private void DisconnectHandler(DisconnectCause cause)
        {
            Debug.Log("Disconnect");
            OnDisconnect?.Invoke();
            Connect();
        }
        
        private void JoinRoomHandler()
        {
            PhotonNetwork.LoadLevel(_defaultSceneName);
        }
        
        public void CreateOrJoin()
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                PhotonNetwork.JoinRandomOrCreateRoom();
            }
        }
        
        public void CreateRoom(string name)
        {
            PhotonNetwork.CreateRoom(name);
        }

        public void JoinRoom(string name)
        {
            PhotonNetwork.JoinRoom(name);
        }
    }
}
