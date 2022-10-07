using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _witPanel;
    [SerializeField] private TMP_InputField _create;
    [SerializeField] private TMP_Dropdown _join;
    [SerializeField] private TMP_Text _joinLabel;

    private void Awake()
    {
        _witPanel.SetActive(true);
        _create.text = "Room1";
        _join.ClearOptions();
    }

    private IEnumerator Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        while (PhotonNetwork.IsConnectedAndReady == false)
        {
            yield return new WaitForSeconds(0.5f);
        }
        _witPanel.SetActive(false);
        while (PhotonNetwork.InLobby == false)
        {
            PhotonNetwork.JoinLobby(TypedLobby.Default);
            yield return new WaitForSeconds(2f);
        }
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(_create.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_join.options[_join.value].text);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }
    
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        var roomNames = (from room in roomList
            where room.IsVisible && room.IsOpen
            orderby room.Name
            select room.Name).ToList();
        _join.ClearOptions();
        if (roomNames.Count == 0)
        {
            _joinLabel.SetText("Search Rooms..");
        }
        else
        {
            _join.AddOptions(roomNames);
        }
    }
}
