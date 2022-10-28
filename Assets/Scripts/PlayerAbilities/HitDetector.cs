using Photon.Pun;
using Photon.Realtime;
using PlayerAbilities;
using UnityEngine;

public abstract class HitDetector : MonoBehaviour
{
    [SerializeField] protected PlayerHealth PlayerHealth;

    [SerializeField] private PlayerPhotonView _playerPhotonView;

    public bool IsMine => _playerPhotonView.IsMine;
    public bool IsBot => _playerPhotonView.IsBot;

    public abstract void DetectHit(float damage, Player player);
}
