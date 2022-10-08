using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class PlayerEnteredRoomCatcher : MonoBehaviour, IInRoomCallbacks
    {
        public event UnityAction<Player> OnEnter, OnLeft, OnMasterSwitch;
        public event UnityAction<Player, Hashtable> OnPlayerPropsUpdate;
        public event UnityAction<Hashtable> OnRoomPropsUpdate;

        public void OnPlayerEnteredRoom(Player newPlayer)
        {
            OnEnter?.Invoke(newPlayer);
        }

        public void OnPlayerLeftRoom(Player otherPlayer)
        {
            OnLeft?.Invoke(otherPlayer);
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
