using Photon.Pun;
using UnityEngine;

namespace Network
{
    public class HideMine : MonoBehaviour
    {
        [SerializeField] private PhotonView _photonView;

        private void Start()
        {
            if (_photonView.IsMine)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
