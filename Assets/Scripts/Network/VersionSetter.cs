using Network.UI;
using Photon.Pun;
using UnityEngine;

namespace Network
{
    public class VersionSetter : MonoBehaviour
    {
        [SerializeField] private string _version;

        private void Awake()
        {
            PhotonNetwork.GameVersion = _version;
        }

        private void Start()
        {
            FindObjectOfType<VersionText>()?.
                SetVersion(_version);
        }
    }
}
