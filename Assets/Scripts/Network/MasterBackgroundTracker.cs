using System;
using System.Collections;
using Agava.WebUtility;
using Photon.Pun;
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

        private void Awake()
        {
            _monitor = GetComponent<MasterClientMonitor>();
            _pingSender = GetComponent<PingSender>();
        }

        private void OnEnable()
        {
            WebApplication.InBackgroundChangeEvent += InBackgroundChangeHandler;
        }

        private void OnDisable()
        {
            WebApplication.InBackgroundChangeEvent -= InBackgroundChangeHandler;
        }
        
        private void OnApplicationPause(bool pause)
        {
            
        }

        private void Update()
        {
            SwitchBackgroundMaster();
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
