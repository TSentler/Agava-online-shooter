using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine.Events;

namespace Network
{
    public class ConnectionCallbackListener : IConnectionCallbacks
    {
        public event UnityAction OnConnect, OnConnectToMaster;
        public event UnityAction<DisconnectCause> OnDisconnnect;
        public event UnityAction<RegionHandler> OnRegionsReceive;
        public event UnityAction<Dictionary<string, object>> OnAuthResponse;
        public event UnityAction<string> OnAuthFail;

        public void OnConnected()
        {
            OnConnect?.Invoke();
        }

        public void OnConnectedToMaster()
        {
            OnConnectToMaster?.Invoke();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            OnDisconnnect?.Invoke(cause);
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            OnRegionsReceive?.Invoke(regionHandler);
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            OnAuthResponse?.Invoke(data);
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
            OnAuthFail?.Invoke(debugMessage);
        }
    }
}
