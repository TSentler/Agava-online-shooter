using Photon.Pun;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class NicknameInput : MonoBehaviour
{
    private TMP_InputField _input;

    private void Awake()
    {
        _input = GetComponent<TMP_InputField>();
        SetNick(_input.text);
        _input.onValueChanged.AddListener(SetNick);      
    }

    private void SetNick(string value)
    {
        PhotonNetwork.NickName = value;
    }
}
