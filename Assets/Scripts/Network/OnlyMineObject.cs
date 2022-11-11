using Photon.Pun;
using UnityEngine;

namespace Network
{
    public class OnlyMineObject : MonoBehaviour
    {
        [SerializeField] private PhotonView _photonView;

        private void Start()
        {
            gameObject.SetActive(_photonView.IsMine);
        }
    }
}
