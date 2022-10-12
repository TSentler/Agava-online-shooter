using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreInput : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;

    private Scoreboard _scoreboard;

    private void Awake()
    {
        _scoreboard = FindObjectOfType<Scoreboard>();
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
}
