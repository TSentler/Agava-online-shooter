using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(TMP_InputField), 
        typeof(PhotonView))]
    public class PlayerNickNameInput : MonoBehaviour
    {
        private TMP_InputField _input;

        private void Awake()
        {
            _input = GetComponent<TMP_InputField>();
        }

        private void OnEnable()
        {
            _input.onValueChanged.AddListener(ValueChangeHandler);
        }

        private void OnDisable()
        {
            _input.onValueChanged.RemoveListener(ValueChangeHandler);
        }

        private void ValueChangeHandler(string nick)
        {
            PhotonNetwork.NickName = nick;
        }
    }
}
