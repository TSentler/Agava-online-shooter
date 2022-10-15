using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreInput : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;

    private MatchEndScoreboard _matchEndScoreboard;
    private Scoreboard _scoreboard;

    private void Awake()
    {
        _scoreboard = FindObjectOfType<Scoreboard>();
        _matchEndScoreboard = FindObjectOfType<MatchEndScoreboard>();
    }

    private void OnEnable()
    {
        _matchEndScoreboard.OnMatchComplete += OnMatchEnd;
    }

    private void OnDisable()
    {
        _matchEndScoreboard.OnMatchComplete -= OnMatchEnd;
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                _scoreboard.OnTabButoonClicked();
            }
        }
    }

    private void OnMatchEnd()
    {
        GetComponent<PlayerScoreInput>().enabled = false;
    }
}
