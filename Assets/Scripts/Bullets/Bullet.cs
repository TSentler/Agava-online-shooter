using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private Vector3 _force;
    [SerializeField] private PhotonView _photonView;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move()
    {
        _rigidbody.AddForce(_force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent(out PlayerHealth playerHealth))
        {
            if(_photonView.IsMine == false)
            {
                playerHealth.ApplyDamage(_damage);
            }
        }
    }
}
