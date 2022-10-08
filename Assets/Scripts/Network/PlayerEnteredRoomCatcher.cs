using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class PlayerEnteredRoomCatcher : MonoBehaviour, IInRoomCallbacks
    {
        public event UnityAction<Player> OnPlayerEnter, OnPlayerLeft, OnMasterSwitch;
        public event UnityAction<Player, Hashtable> OnPlayerPropsUpdate;
        public event UnityAction<Hashtable> OnRoomPropsUpdate;

        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            OnPlayerEnter?.Invoke(newPlayer);
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            OnPlayerLeft?.Invoke(otherPlayer);
        }

        public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            OnRoomPropsUpdate?.Invoke(propertiesThatChanged);
        }

        public void OnPlayerPropertiesUpdate(Player targetPlayer, 
            Hashtable changedProps)
        {
            OnPlayerPropsUpdate?.Invoke(targetPlayer, changedProps);
        }

        public void OnMasterClientSwitched(Player newMasterClient)
        {
            OnMasterSwitch?.Invoke(newMasterClient);
        }
    }
}
