using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RoomItemView : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _roomText;

    private RoomInfo _roomInfo;

    public void Render(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        _roomText.text = roomInfo.Name;
    }

    public void OnClick()
    {
        ConnectToServer.Instance.JoinRoom(_roomInfo);
    }
}
