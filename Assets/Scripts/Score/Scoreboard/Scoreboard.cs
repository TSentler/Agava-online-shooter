using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class Scoreboard : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] private ScoreboardItem _scoreItemTemplate;
    [SerializeField] private Transform _container;
    [SerializeField] private GameObject _panel;
    [SerializeField] private PhotonView _photonView;

    private Dictionary<Player, ScoreboardItem> _playersSocres = new Dictionary<Player, ScoreboardItem>();

    //private void Awake()
    //{
    //    foreach (Player player in PhotonNetwork.PlayerList)
    //    {
    //        AddScore(player);
    //    }
    //}

    private void Update()
    {
        if(_photonView == null)
        {
            return;
        }

        if (_photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
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

        }
    }

    private void AddScore(Player player)
    {
        ScoreboardItem item = Instantiate(_scoreItemTemplate, _container);
        item.Initialize(player);
        _playersSocres.Add(player, item);
    }

    private void DeleteScore(Player player)
    {
        _playersSocres.Remove(player);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        AddScore(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        DeleteScore(otherPlayer);
    }

    private void OpenPanel()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScore(player);
        }

        _panel.SetActive(true);
    }

    private void ClosePanel()
    {
        foreach (var scoreItem in _playersSocres)
        {
            Destroy(scoreItem.Value.gameObject);
        }

        _playersSocres.Clear();
        _panel.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }
}
