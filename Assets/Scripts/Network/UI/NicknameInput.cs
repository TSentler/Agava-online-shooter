using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Network.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class NicknameInput : MonoBehaviour
    {
        private TMP_InputField _input;

        private void Awake()
        {
            _input = GetComponent<TMP_InputField>();
            _input.onValueChanged.AddListener(SetNick);
            SetNick(_input.text);
        }

        private void SetNick(string value)
        {
            PhotonNetwork.NickName = value;
        }
    }
}
