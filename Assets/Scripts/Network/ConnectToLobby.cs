using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Network
{
    public class ConnectToLobby : MonoBehaviourPunCallbacks
    {
        private Coroutine _joinLobbyCoroutine;

        private void JoinLobby()
        {
            if (_joinLobbyCoroutine != null)
                return;

            _joinLobbyCoroutine = 
                StartCoroutine(JoinLobbyCoroutine());
        }
        
        private IEnumerator JoinLobbyCoroutine()
        {
            while (PhotonNetwork.InLobby == false)
            {
                Debug.Log("TryJoinLobby");
                PhotonNetwork.JoinLobby();
                yield return new WaitForSeconds(2f);
            }
            _joinLobbyCoroutine = null;
        }

        public override void OnConnectedToMaster()
        {
            JoinLobby();
        }

        public override void OnLeftLobby()
        {
            Debug.Log("left lobby");
        }
    }
}
