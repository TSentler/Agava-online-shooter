using Network;
using Network.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MatchEndScoreboard : MonoBehaviour
{
    [SerializeField] private GameObject _matchEndPanel;
    [SerializeField] private Transform _fartherPanel;
    [SerializeField] private ScoreboardItem _scoreTemplate;

    private Dictionary<Player, int> _sortedScores = new Dictionary<Player, int>();
    private MatchmakingCallbacksCatcher _matchCallbacks;
    
    public Action MatchComplete;
    
    public void OpenPanel()
    {
        _matchEndPanel.SetActive(true);
        MatchComplete?.Invoke();

        foreach (var player in PhotonNetwork.PlayerList)
        {
            _sortedScores.Add(player, (int)player.CustomProperties["Kills"]);
        }

        var scoreSort = _sortedScores.OrderByDescending(x => x.Value);

        foreach (var score in scoreSort)
        {
            ScoreboardItem item = Instantiate(_scoreTemplate, _fartherPanel);
            item.Initialize(score.Key);
        }

        Cursor.lockState = CursorLockMode.None;
        PhotonNetwork.CurrentRoom.IsOpen = false;
    }

    public void OnRestartButtonClick()
    {
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);   
    }

    public void OnExitButtonClick()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void Awake()
    {
        _matchCallbacks = FindObjectOfType<MatchmakingCallbacksCatcher>();
    }

    private void OnEnable()
    {
        _matchCallbacks.OnRoomLeft += RoomLeftHandler;
    }

    private void OnDisable()
    {
        _matchCallbacks.OnRoomLeft -= RoomLeftHandler;
    }

    private void RoomLeftHandler()
    {
        PhotonNetwork.LoadLevel(0);
    }
}
