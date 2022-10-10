using System.Collections;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]

public class PlayerHealth : MonoBehaviour
{
    private PhotonView _view;
    private float _currentHealth = MaxHealth;

    private const float MaxHealth = 100f;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    public void TakeDamage(float damage)
    {
        _view.RPC(nameof(TakeDamageRPC), RpcTarget.All, damage);
    }

    [PunRPC]
    private void TakeDamageRPC(float damage)
    {
        if (_view.IsMine == false)
            return;

        if (CanDecreaseHealth(damage))
        {
            _currentHealth -= damage;
            Debug.Log(damage);
        }
        else
        {
            Die();
        }

    }

    private bool CanDecreaseHealth(float damage)
    {
        return _currentHealth - damage > 0;
    }

    private void Die()
    {
        Debug.Log("Die");
    }
}
