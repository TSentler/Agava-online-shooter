using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuInput : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;

    private bool _isOpen;
    private OpenMenu _openMenu;

    public bool IsOpen => _isOpen;

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
            _isOpen = _openMenu.IsOpen;
        }
    }
}
