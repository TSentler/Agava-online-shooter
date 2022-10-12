using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _controlPanel;
    [SerializeField] private GameObject _progressLabel;
    [SerializeField] private TMP_InputField _inputField;

    private string _gameVersion = "1";
    private bool isConnecting;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _progressLabel.SetActive(false);
        _controlPanel.SetActive(true);
    }

    public void Connect()
    {
        _progressLabel.SetActive(true);
        _controlPanel.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
            PhotonNetwork.NickName = _inputField.text;
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomOrCreateRoom();
            PhotonNetwork.NickName = _inputField.text;
            isConnecting = false;
            
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        _progressLabel.SetActive(false);
        _controlPanel.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'Room for 1' ");
            PhotonNetwork.LoadLevel("Room1");
        }
    }
}
