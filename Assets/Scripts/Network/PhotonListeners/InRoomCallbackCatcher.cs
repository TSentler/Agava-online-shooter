using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace Network
{
    public class InRoomCallbackCatcher : MonoBehaviour
    {
        private InRoomCallbackListener _inRoomCallbacks = new();
        
        public event UnityAction<Player> OnPlayerEnter
        {
            add => _inRoomCallbacks.OnPlayerEnter += value;
            remove => _inRoomCallbacks.OnPlayerEnter -= value;
        }
        public event UnityAction<Player> OnPlayerLeft
        {
            add => _inRoomCallbacks.OnPlayerLeft += value;
            remove => _inRoomCallbacks.OnPlayerLeft -= value;
        }
        public event UnityAction<Player> OnMasterSwitch
        { 
            add => _inRoomCallbacks.OnMasterSwitch += value;
            remove => _inRoomCallbacks.OnMasterSwitch -= value;
        }
        public event UnityAction<Player, Hashtable> OnPlayerPropsUpdate
        {
            add => _inRoomCallbacks.OnPlayerPropsUpdate += value;
            remove => _inRoomCallbacks.OnPlayerPropsUpdate -= value;
        }
        public event UnityAction<Hashtable> OnRoomPropsUpdate
        {
            add => _inRoomCallbacks.OnRoomPropsUpdate += value;
            remove => _inRoomCallbacks.OnRoomPropsUpdate -= value;
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(_inRoomCallbacks);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(_inRoomCallbacks);
        }
    }
}
