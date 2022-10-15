using Network;
using Network.UI;
using Photon.Pun;
using System;
using Unity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchEndScoreboard : MonoBehaviour
{
    [SerializeField] private GameObject _matchEndPanel;
    [SerializeField] private Transform _fartherPanel;
    [SerializeField] private ScoreboardItem _scoreTemplate;
    [SerializeField] private ConnectionCallbackCatcher _destroyGameObjects;
    [SerializeField] private MasterClientMonitor _masterClientMonitor;
    [SerializeField] private VersionText _versionText;

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
            ScoreboardItem item = Instantiate(_scoreTemplate, _fartherPanel);
            item.Initialize(player);
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
        Destroy(_masterClientMonitor.gameObject);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}
