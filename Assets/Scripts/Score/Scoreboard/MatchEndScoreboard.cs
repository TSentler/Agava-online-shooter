using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MatchEndScoreboard : MonoBehaviour
{
    [SerializeField] private GameObject _matchEndPanel;
    [SerializeField] private Transform _fartherPanel;
    [SerializeField] private ScoreboardItem _scoreTemplate;

    public event UnityAction OnMatchComplete;

    public void OpenPanel()
    {
        _matchEndPanel.SetActive(true);
        OnMatchComplete?.Invoke();

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
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }
}
