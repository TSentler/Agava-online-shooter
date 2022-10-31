using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuInput : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;

    private OpenMenu _openMenu;

    private void Awake()
    {
        _openMenu = FindObjectOfType<OpenMenu>();
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _openMenu.MenuButtonClick();
            }
        }
    }
}
