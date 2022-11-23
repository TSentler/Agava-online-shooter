using Photon.Pun;
using Score;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroop;
    private MatchEndScoreboard _matchEndScoreBoard;

    private bool _isOpen = false;

    public bool IsOpen => _isOpen;

    private void Start()
    {
        _canvasGroop.alpha = 0f;
        _matchEndScoreBoard = FindObjectOfType<MatchEndScoreboard>();
    }

    public void MenuButtonClick()
    {
        if (_matchEndScoreBoard.CanPlay == false)
            return;

        if (_canvasGroop.alpha == 0)
        {
            OpenMenuPanel();
        }
        else
        {
            CloseMenu();
        }
    }

    public void OpenMenuPanel()
    {
        Cursor.lockState = CursorLockMode.None;
        _canvasGroop.alpha = 1;
        _isOpen = true;
    }

    private void CloseMenu()
    {
        _canvasGroop.alpha = 0;
        Cursor.lockState = CursorLockMode.Locked;
        _isOpen = false;
    }

    public void Open()
    {
        _isOpen = true;
    }

    public void Close()
    {
        _isOpen = false;
    }
}
