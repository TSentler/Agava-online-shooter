using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _waitPanel;
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _roomMenu;
    [SerializeField] private GameObject _findRoomMenu;
    [SerializeField] private Transform _roomItemViewContent;
    [SerializeField] private Transform _roomPlayerViewContent;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TMP_InputField _inputFieldName;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private PlayerItemView _playerItemTemplate;
    [SerializeField] private RoomItemView _roomItemTemplate;

    private List<RoomItemView> _roomItemViews = new List<RoomItemView>();
    private string _gameVersion = "1";

    private void Awake()
    {
        _waitPanel.SetActive(true);
        SetNick(_inputFieldName.text);
        _inputFieldName.onValueChanged.AddListener(SetNick);
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

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        _waitPanel.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        _roomMenu.SetActive(true);
        _waitPanel.SetActive(false);
        _findRoomMenu.SetActive(false);
        _roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in _roomPlayerViewContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            var view = Instantiate(_playerItemTemplate, _roomPlayerViewContent);
            view.Render(players[i], _inputFieldName.text);
        }

        _startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);
        _startGameButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        _waitPanel.SetActive(false);
        _mainMenu.SetActive(true);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        foreach (var item in _roomItemViews)
        {
            item.Click -= JoinRoom;
            Destroy(item.gameObject);
        }

        _roomItemViews.Clear();

        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            {
                continue;
            }

            var view = Instantiate(_roomItemTemplate, _roomItemViewContent);
            view.Render(roomList[i]);
            view.Click += JoinRoom;
            _roomItemViews.Add(view);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        var view = Instantiate(_playerItemTemplate, _roomPlayerViewContent);
        view.Render(newPlayer, _inputFieldName.text);
    }

    public void JoinOrCreateRoom()
    {
        string roomName;

        if (_roomItemViews.Count != 0)
        {
            roomName = _roomItemViews[0].RoomInfo.Name;
        }
        else
        {
            roomName = "Room" + Random.Range(0, 1000).ToString();
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;

        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
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

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        _waitPanel.SetActive(true);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        _roomMenu.SetActive(false);
        _waitPanel.SetActive(true);
    }

    public void StartGeme()
    {
        PhotonNetwork.LoadLevel(1);
    }

    private void SetNick(string value)
    {
        PhotonNetwork.NickName = value;
    }
}
