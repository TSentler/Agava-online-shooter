using Photon.Pun;
using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _health;
    [SerializeField] private PhotonView _photonView;

    public float Health => _health;

    public UnityAction<float> ChangeHealth;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }

    public void ApplyDamage(float damage)
    {
        if(damage < 0)
        {
            throw new ArgumentOutOfRangeException("Damage can't be negative");
        }

        if (_photonView.IsMine == true)
        {
            _health -= damage;
            ChangeHealth?.Invoke(_health);
        }

        if (_photonView.IsMine)
        {
            if (_health <= 0)
            {
                PhotonNetwork.Destroy(gameObject);
                Debug.Log("Устрой дестрой");
            }
        }     
    }
}
