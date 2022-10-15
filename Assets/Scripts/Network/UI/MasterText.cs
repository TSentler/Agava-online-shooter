using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Network.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class MasterText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        private void Update()
        {
            if (PhotonNetwork.MasterClient != null)
            {
                var text = PhotonNetwork.IsMasterClient
                    ? "I`am"
                    : "Not me";
                _text.SetText("Master is " + 
                              PhotonNetwork.MasterClient.NickName + 
                              " " + text);
            }
        }
    }
}
