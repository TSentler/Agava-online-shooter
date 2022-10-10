using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NiknameView : MonoBehaviour, IPunObservable
{
    [SerializeField] private PhotonView _photonViev;

    private TMP_Text _text;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();

        if (_photonViev.IsMine)
        {
            _text.text = PhotonNetwork.NickName;
        }
        else
        {
            _text.text = _photonViev.Owner.NickName;
        }
    }
}
