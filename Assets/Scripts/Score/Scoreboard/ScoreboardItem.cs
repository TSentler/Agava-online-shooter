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
        _nickname.text = player.NickName;
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
       
    //}
}
