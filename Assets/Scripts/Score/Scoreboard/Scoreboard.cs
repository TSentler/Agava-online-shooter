using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Scoreboard : MonoBehaviourPunCallbacks/*, IPunObservable*/
{
    [SerializeField] private ScoreboardItem _scoreItemTemplate;
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _panel;
    [SerializeField] private PhotonView _photonView;

    private Dictionary<Player, ScoreboardItem> _playersScores = new Dictionary<Player, ScoreboardItem>();

    //private void Update()
    //{
    //    //if(_photonView == null)
    //    //{
    //    //    return;
    //    //}

    //    //Debug.Log(_photonView.IsMine);

    //    if (_photonView.IsMine)
    //    { 


    //    }
    //}

    private void AddScore(Player player)
    {
        ScoreboardItem item = Instantiate(_scoreItemTemplate, _container);
        item.Initialize(player);

        if (_playersScores.ContainsKey(player) == false)
        {
            _playersScores.Add(player, item);
        }

        //foreach(var player1 in PhotonNetwork.PlayerList)
        //{
        //    Debug.Log(player1);
        //}

        //foreach(var player2 in _playersScores)
        //{
        //    Debug.Log(player2.Key);
        //    Debug.Log(player2.Value); 
        //}
    }

    private void DeleteScore(Player player)
    {
        _playersScores.Remove(player);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        AddScore(newPlayer);
        //_photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        DeleteScore(otherPlayer);
    }

    public void OnTabButoonClicked()
    {
        if (_panel.activeSelf == false)
        {
            OpenPanel();
        }
        else
        {
            ClosePanel();
        }

    }

    private void OpenPanel()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScore(player);
        }

        Debug.Log(PhotonNetwork.PlayerList.Length);
        _panel.SetActive(true);
    }

    private void ClosePanel()
    {
        foreach (var scoreItem in _playersScores)
        {
            Destroy(scoreItem.Value.gameObject);
        }

        _playersScores.Clear();
        _panel.SetActive(false);
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{

    //}
}
