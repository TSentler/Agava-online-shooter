using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;

public class Scoreboard : MonoBehaviourPunCallbacks
{
    [SerializeField] private ScoreboardItem _scoreItemTemplate;
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _panel;
    [SerializeField] private PhotonView _photonView;

    private Dictionary<Player, ScoreboardItem> _playersScores = new Dictionary<Player, ScoreboardItem>();
    private Dictionary<Player, int> _sortedScores = new Dictionary<Player, int>();

    private void DeleteScore(Player player)
    {
        Destroy(_playersScores[player]);
        _playersScores.Remove(player);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
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
            _sortedScores.Add(player, (int)player.CustomProperties["Kills"]);
        }

        var scoreSort = _sortedScores.OrderByDescending(x => x.Value);

        foreach(var score in scoreSort)
        {
            ScoreboardItem item = Instantiate(_scoreItemTemplate, _container);
            item.Initialize(score.Key);
            _playersScores.Add(score.Key, item);
        }

        _sortedScores.Clear();
        _panel.SetActive(true);
    }

    private void ClosePanel()
    {
        foreach (var scrore in _playersScores)
        {
            Destroy(scrore.Value.gameObject);
        }

        _playersScores.Clear();
        _panel.SetActive(false);
    }
}
