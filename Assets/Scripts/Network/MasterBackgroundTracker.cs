using System.Collections;
using Agava.WebUtility;
using Photon.Pun;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(MasterClientMonitor))]
    public class MasterBackgroundTracker : MonoBehaviour
    {
        private readonly float _repeatRequestDelay = 3f;
        
        private bool _inBackground;
        private MasterClientMonitor _monitor;
        private Coroutine _coroutine;

        private void Awake()
        {
            _monitor = GetComponent<MasterClientMonitor>();
        }

        private void OnEnable()
        {
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChange;
        }

        private void OnDisable()
        {
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChange;
        }
        
        private void OnApplicationPause(bool pause)
        {
            
        }
        
        private void OnInBackgroundChange(bool isBack)
        {
            _inBackground = isBack;
            if (_inBackground && _coroutine == null)
            {
                _coroutine = StartCoroutine(
                    SwitchBackgroundMasterCoroutine());
            }
        }

        private IEnumerator SwitchBackgroundMasterCoroutine()
        {
            while (_inBackground && PhotonNetwork.IsMasterClient)
            {
                _monitor.LocallyHandOffMasterClient();
                yield return new WaitForSeconds(_repeatRequestDelay);
            }
            _coroutine = null;
        }
    }
}
