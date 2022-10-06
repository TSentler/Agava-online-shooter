using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _witPanel;
    [SerializeField] private TMP_InputField _create, _join;

    private void Awake()
    {
        _witPanel.SetActive(true);
        _create.text = "Room1";
        _join.text = "Room1";
    }

    private IEnumerator Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        while (PhotonNetwork.IsConnectedAndReady == false)
        {
            yield return new WaitForSeconds(0.5f);
        }
        _witPanel.SetActive(false);
    }

    public void CreateRoom()
    {
        Debug.Log(_create.text);
        PhotonNetwork.CreateRoom(_create.text);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_join.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
