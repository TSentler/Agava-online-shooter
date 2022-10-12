using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardItem : MonoBehaviour/*, IPunObservable*/
{
    [SerializeField] private TMP_Text _nickname;
    [SerializeField] private TMP_Text _death;
    [SerializeField] private TMP_Text _kills;

    public void Initialize(Player player)
    {
        if (player == null)
            return;

        _nickname.text = player.NickName;
        _death.text = player.CustomProperties["Death"].ToString();
        _kills.text = player.CustomProperties["Kils"].ToString();
        
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
       
    //}
}
