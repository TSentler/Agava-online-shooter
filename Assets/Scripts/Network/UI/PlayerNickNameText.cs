using Photon.Pun;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class PlayerNickNameText : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        if (_photonView.IsMine)
        {
            _text.text = PhotonNetwork.NickName;
            gameObject.SetActive(false);
        }
        else
        {
            _text.text = _photonView.Owner.NickName;
        }
    }
}
