using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public static ConnectToServer Instance;

    [SerializeField] private GameObject _waitPanel;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _roomMenu;
    [SerializeField] private GameObject _findRoomMenu;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private Transform _roomItemViewContent;
    [SerializeField] private RoomItemView _roomItemTemplate;

    private string _gameVersion = "1";

    private void Awake()
    {
        Instance = this;
        PhotonNetwork.AutomaticallySyncScene = true;
        _waitPanel.SetActive(true);
    }

    private IEnumerator Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        while (PhotonNetwork.IsConnectedAndReady == false)
        {
            yield return new WaitForSeconds(0.5f);
        }
        _waitPanel.SetActive(false);
        PhotonNetwork.GameVersion = _gameVersion;
    }

    public void FindRoomList()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(_roomNameInputField.text))
        {
            _roomNameInputField.text = "Room" + Random.Range(0, 1000).ToString();
        }

        PhotonNetwork.CreateRoom(_roomNameInputField.text);
        _waitPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        _waitPanel.SetActive(false);
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        _waitPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        _roomMenu.SetActive(true);
        _waitPanel.SetActive(false);
        _findRoomMenu.SetActive(false);
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;      
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        _roomMenu.SetActive(false);
        _waitPanel.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        _waitPanel.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform item in _roomItemViewContent)
        {
            Destroy(item.gameObject);
        }

        for (int i = 0; i < roomList.Count; i++)
        {
            Debug.Log(i);
            Instantiate(_roomItemTemplate, _roomItemViewContent).Render(roomList[i]);           
        }
    }
}
