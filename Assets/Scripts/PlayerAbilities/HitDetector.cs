using Photon.Pun;
using Photon.Realtime;
using PlayerAbilities;
using UnityEngine;

public abstract class HitDetector : MonoBehaviour
{
    [SerializeField] protected PlayerHealth PlayerHealth;

    [SerializeField] private PlayerInfo _playerInfo;

    public bool IsMine => _playerInfo.IsMine;
    public bool IsBot => _playerInfo.IsBot;

    public abstract void DetectHit(float damage, Player player);
}
