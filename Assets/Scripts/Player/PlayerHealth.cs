using Photon.Pun;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _health;
    [SerializeField] private PhotonView _photonView;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }

    public void ApplyDamage(float damage)
    {
        if(damage < 0)
        {
            throw new ArgumentOutOfRangeException("Damage can't be negative");
        }

        if (_photonView.IsMine == false)
        {
            _health -= damage;
            Debug.Log(_health);

            if(_health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
