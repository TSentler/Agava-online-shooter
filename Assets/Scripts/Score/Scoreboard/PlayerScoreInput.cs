using Photon.Pun;
using Score;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreInput : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private GameObject _playerHealth;

    private MatchEndScoreboard _matchEndScoreboard;
    private Scoreboard _scoreboard;

    private void Awake()
    {
        _scoreboard = FindObjectOfType<Scoreboard>();
        _matchEndScoreboard = FindObjectOfType<MatchEndScoreboard>();
    }

    private void OnEnable()
    {
        _matchEndScoreboard.MatchComplete += OnMatchEnd;
    }

    private void OnDisable()
    {
        _matchEndScoreboard.MatchComplete -= OnMatchEnd;
    }

    private void Update()
    {
        //_matchEndScoreboard = FindObjectOfType<MatchEndScoreboard>();

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
        if (_playerHealth == null)
        {
            Debug.LogWarning("PlayerHealth is NULL", this);
            return;
        }
        _playerHealth.SetActive(false);
    }
}
