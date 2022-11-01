using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroop;

    private void Start()
    {
        _canvasGroop.alpha = 0f;
    }

    public void MenuButtonClick()
    {
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
    }

    private void CloseMenu()
    {
        _canvasGroop.alpha = 0;
        Cursor.lockState = CursorLockMode.Locked;
    }
}