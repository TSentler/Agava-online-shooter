using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class Health : MonoBehaviour
{
    private HealthText _text;
    private PhotonView _view;
    private float _health;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        _text = FindObjectOfType<HealthText>();
    }

    private void Start()
    {
        _text.SetHealth(_health);
    }

    [PunRPC]
    private void TakeDamageRPC()
    {
        if (_view.IsMine == false)
            return;
        
        _health++;
        _text.SetHealth(_health);
    }

    public void TakeDamage()
    {
        _view.RPC(nameof(TakeDamageRPC), RpcTarget.All);
    }
}
