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

    [PunRPC]
    private void AddScore(Player player)
    {
        if (_playersScores.ContainsKey(player) == false)
        {
            ScoreboardItem item = Instantiate(_scoreItemTemplate, _container);
            item.Initialize(player);
            _playersScores.Add(player, item);
        }
    }

    private void DeleteScore(Player player)
    {
        Destroy(_playersScores[player]);
        _playersScores.Remove(player);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        _photonView.RPC(nameof(AddScore), RpcTarget.AllBuffered, newPlayer);
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
          _photonView.RPC(nameof(AddScore),RpcTarget.AllBuffered, player);
        }

        Debug.Log(PhotonNetwork.PlayerList.Length);
        _panel.SetActive(true);
    }

    private void ClosePanel()
    {
        _photonView.RPC(nameof(PUN_ClosePanel), RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void PUN_ClosePanel()
    {
        foreach (var scrore in _playersScores)
        {
            Destroy(scrore.Value.gameObject);
        }

        _playersScores.Clear();
        _panel.SetActive(false);
    }
}
