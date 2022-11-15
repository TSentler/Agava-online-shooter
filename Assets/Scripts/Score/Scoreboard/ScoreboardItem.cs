using Lean.Localization;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardItem : MonoBehaviour 
{ 
    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private TMP_Text _death;
    [SerializeField] private TMP_Text _kills;

    private int _killsCount;

    public int KillsCount => _killsCount;

    public void Initialize(Player player)
    {
        if (player == null)
            return;

        if(player.CustomProperties.ContainsKey("Death") && player.CustomProperties.ContainsKey("Kills"))
        {
            _nickname.text = player.NickName;
            string death = LeanLocalization.GetTranslationText("Death");
            string kills = LeanLocalization.GetTranslationText("Kills");
            _death.text = death + player.CustomProperties["Death"].ToString();
            _killsCount = (int)player.CustomProperties["Kills"];
            _kills.text = kills + _killsCount;
        }
        else
        {
            Destroy(gameObject);
        }      
    }
}
