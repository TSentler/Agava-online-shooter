using ExitGames.Client.Photon;
using Network;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

namespace Match
{
    public class TimeSyncronizer : MonoBehaviour
    {
        private readonly string _startTimeName = "StartTime";

        private double _startTime = -1f;
        private InRoomCallbackCatcher _roomCatcher;

        public event UnityAction<float> OnTimerSync;

        public bool IsReady => _startTime >= 0d;
        
        private void Awake()
        {
            _roomCatcher = FindObjectOfType<InRoomCallbackCatcher>();
            var props = PhotonNetwork.CurrentRoom.CustomProperties;

            if (props.ContainsKey(_startTimeName))
            {
                SetTime((double)props[_startTimeName]);
            }
            else if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(
                    new Hashtable
                    { { _startTimeName, PhotonNetwork.Time } });
            }
        }

        private void OnEnable()
        {
            _roomCatcher.OnRoomPropsUpdate += RoomPropsUpdateHandler;
        }

        private void OnDisable()
        {
            _roomCatcher.OnRoomPropsUpdate -= RoomPropsUpdateHandler;
        }

        private void RoomPropsUpdateHandler(Hashtable props)
        {
            if (props.ContainsKey(_startTimeName) == false)
                return;
            
            var startTime = (double)props[_startTimeName];
            float diff = (float)(_startTime - startTime);
            if (_startTime > 0d && Mathf.Approximately(diff, 0f))
                return;
            
            SetTime(startTime);
        }

        private void SetTime(double value)
        {
            _startTime = value;
            float timePassed = (float)(PhotonNetwork.Time - _startTime);
            OnTimerSync?.Invoke(timePassed);
        }

        public float GetTimePassed()
        {
            return (float)(PhotonNetwork.Time - _startTime);
        }
    }
}
