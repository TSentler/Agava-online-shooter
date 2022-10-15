using System.Collections;
using Agava.WebUtility;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(MasterClientMonitor),
        typeof(PingSender))]
    public class MasterBackgroundTracker : MonoBehaviour
    {
        private readonly float _repeatRequestDelay = 3f;
        
        private bool _inBackground;
        private MasterClientMonitor _monitor;
        private PingSender _pingSender;
        private Coroutine _coroutine;
        private InRoomCallbackCatcher _enteredCallbackCatcher;

        private void Awake()
        {
            _enteredCallbackCatcher = FindObjectOfType<InRoomCallbackCatcher>();
            _monitor = GetComponent<MasterClientMonitor>();
            _pingSender = GetComponent<PingSender>();
        }

        private void OnEnable()
        {
            _enteredCallbackCatcher.OnMasterSwitch += MasterSwitchHandler;
            WebApplication.InBackgroundChangeEvent += InBackgroundChangeHandler;
        }

        private void OnDisable()
        {
            _enteredCallbackCatcher.OnMasterSwitch -= MasterSwitchHandler;
            WebApplication.InBackgroundChangeEvent -= InBackgroundChangeHandler;
        }

        private void MasterSwitchHandler(Player player)
        {
            SwitchBackgroundMaster();
        }

        private void OnApplicationPause(bool pause)
        {
            
        }

        private void SwitchBackgroundMaster()
        {
            if (_inBackground
                && PhotonNetwork.IsMasterClient && _coroutine == null)
            {
                _coroutine = StartCoroutine(
                    SwitchBackgroundMasterCoroutine());
            }
        }

        private void InBackgroundChangeHandler(bool isBack)
        {
            _inBackground = isBack;
            SwitchBackgroundMaster();
        }

        private IEnumerator SwitchBackgroundMasterCoroutine()
        {
            while (_inBackground && PhotonNetwork.IsMasterClient)
            {
                _pingSender.SendPingImmidiate();
                _monitor.LocallyHandOffMasterClient();
                yield return new WaitForSeconds(_repeatRequestDelay);
            }
            _coroutine = null;
        }
    }
}
