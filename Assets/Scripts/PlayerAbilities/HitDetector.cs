using Photon.Pun;
using Photon.Realtime;
using PlayerAbilities;
using UnityEngine;

public abstract class HitDetector : MonoBehaviour
{
    [SerializeField] protected PlayerHealth PlayerHealth;

    [SerializeField] private PhotonView _photonView;

    public PhotonView PhotonView => _photonView;

    public abstract void DetectHit(float damage, Player player);
}
