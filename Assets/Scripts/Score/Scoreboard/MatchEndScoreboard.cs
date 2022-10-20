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
    [SerializeField] private MasterClientMonitor _masterClientMonitor;

    private Dictionary<Player, int> _sortedScores = new Dictionary<Player, int>();

    public Action MatchComplete;

    private void Awake()
    {
        _masterClientMonitor = FindObjectOfType<MasterClientMonitor>();
    }

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
        Debug.Log("Restart");
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);   
    }

    public void OnExitButtonClick()
    {
        Debug.Log("Exit");
        Destroy(_masterClientMonitor.gameObject);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);       
    }
}
