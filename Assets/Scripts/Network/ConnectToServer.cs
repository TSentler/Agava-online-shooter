using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        private Coroutine _connectCoroutine;

        public event UnityAction OnConnectStart, OnConnectEnd, OnDisconnect;
        
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
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
            OnDisconnect?.Invoke();
            Connect();
        }
    }
}
