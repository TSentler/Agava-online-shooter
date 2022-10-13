using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchEndScoreboard : MonoBehaviour
{
    [SerializeField] private GameObject _matchEndPanel;
    [SerializeField] private Transform _fartherPanel;
    [SerializeField] private ScoreboardItem _scoreTemplate;

    public Action MatchComplete;

    public void OpenPanel()
    {
        _matchEndPanel.SetActive(true);
        MatchComplete?.Invoke();

        foreach(var player in PhotonNetwork.PlayerList)
        {
            ScoreboardItem item = Instantiate(_scoreTemplate, _fartherPanel);
            item.Initialize(player);
        }
    }

    public void OnRestartButtonClick()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public void OnExitButtonClick()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}
