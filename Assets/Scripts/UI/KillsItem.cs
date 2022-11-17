using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillsItem : MonoBehaviour
{
    [SerializeField] private TMP_Text _killerName;
    [SerializeField] private TMP_Text _deadPlayerName;

    public void InstantiateKills(string killerName, string deadPlayerName)
    {
        _killerName.text = killerName;
        _deadPlayerName.text = deadPlayerName;
    }
}
