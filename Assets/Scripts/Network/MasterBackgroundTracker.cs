using Agava.WebUtility;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(MasterClientMonitor))]
    public class MasterBackgroundTracker : MonoBehaviour
    {
        private MasterClientMonitor _monitor;

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
            if (pause == false)
                return;
            
            // SwitchBackgroundMaster();
        }
        
        private void OnInBackgroundChange(bool isBack)
        {
            SwitchBackgroundMaster();
        }

        private void SwitchBackgroundMaster()
        {
            _monitor.LocallyHandOffMasterClient();
        }
    }
}
