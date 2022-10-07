using Photon.Pun;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text), 
    typeof(PhotonView))]
public class PlayerNickName : MonoBehaviour
{
    private PhotonView _view;
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
        _view = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (_view.IsMine)
        {
            _text.text = PhotonNetwork.NickName;
        }
        else
        {
            _text.text = _view.Owner.NickName;
        }
    }
}
