using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerNameInputFild : MonoBehaviour
{
    private const string PlayerNameKey = "PlayerName";
    private InputField _inpurField;

    private void Awake()
    {
        _inpurField = GetComponent<InputField>();
        string defaultName = "Anonymous";

        if (_inpurField != null)
        {
            if (PlayerPrefs.HasKey(PlayerNameKey))
            {
                defaultName = PlayerPrefs.GetString(PlayerNameKey);
                _inpurField.text = defaultName;
            }
        }

        PhotonNetwork.NickName = defaultName;
    }


    public void SetPlayerName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("Name is empty");
            return;
        }
        else
        {
            PhotonNetwork.NickName = name;
            PlayerPrefs.SetString(PlayerNameKey, name);
        }
    }
}
