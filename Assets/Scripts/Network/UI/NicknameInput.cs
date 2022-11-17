using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Network.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class NicknameInput : MonoBehaviour
    {
        [SerializeField] private List<string> _nickNames;

        private TMP_InputField _input;

        private void Awake()
        {
            _input = GetComponent<TMP_InputField>();
            _input.onValueChanged.AddListener(SetNick);
            _input.text = GetRandomNickName();
            SetNick(_input.text);
        }

        private void SetNick(string value)
        {
            PhotonNetwork.NickName = value;
        }

        private string GetRandomNickName()
        {
            int nickNameIndex = Random.Range(0, _nickNames.Count);

            return _nickNames[nickNameIndex];
        }
    }
}
