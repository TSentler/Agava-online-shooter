using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        private Coroutine _connectCoroutine;

        [SerializeField] private string _version;

        public event UnityAction OnConnectStart, OnConnectEnd;
        
        public string Version => _version;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.GameVersion = _version;
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
        
        public void CreateRoom(string name)
        {
            PhotonNetwork.CreateRoom(name);
        }

        public void JoinRoom(string name)
        {
            PhotonNetwork.JoinRoom(name);
        }

        public override void OnJoinedRoom()
        {
            PhotonNetwork.LoadLevel("Room1");
        }
        
        public override void OnConnectedToMaster()
        {
            OnConnectEnd?.Invoke();
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Disconnect");
            Connect();
        }
    }
}
