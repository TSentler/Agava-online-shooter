using Photon.Pun;
using UnityEngine;
using System;
using UnityEngine.Events;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _health;
    [SerializeField] private PhotonView _photonView;

    private int _kills;
    private int _deaths;

    public int Deaths => _deaths;
    private int Kills => _kills;
    public float Health => _health;

    public UnityAction<float> ChangeHealth;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }

    private void Awake()
    {
        _deaths = 0;
        _kills = 0;
        PhotonNetwork.SetPlayerCustomProperties(new Hashtable() { { "Death", _deaths } });
        PhotonNetwork.SetPlayerCustomProperties(new Hashtable() { { "Kils", _kills } });
    }

    public void ApplyDamage(float damage, Player player)
    {
        object[] rpcParametrs = new object[2] { damage, player};
        _photonView.RPC(nameof(ApplyDamageRPC), RpcTarget.All, rpcParametrs);
    }

    [PunRPC]
    private void ApplyDamageRPC(float damage, Player player)
    {
        if (_photonView.IsMine == false)
        {
            return;
        }

        if (damage < 0)
        {
            throw new ArgumentOutOfRangeException("Damage can't be negative");
        }

        if (_photonView.IsMine)
        {
            _health -= damage;
            ChangeHealth?.Invoke(_health);

            if (_health <= 0)
            {
                _deaths++;
                PhotonNetwork.SetPlayerCustomProperties(new Hashtable() { { "Death", _deaths } });
                int kills = (int)player.CustomProperties["Kils"];
                player.SetCustomProperties(new Hashtable() { { "Kils", kills++ } });
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("Устрой дестрой");
            }
        }
    }
}