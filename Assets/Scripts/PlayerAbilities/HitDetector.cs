using Photon.Pun;
using Photon.Realtime;
using PlayerAbilities;
using UnityEngine;

public abstract class HitDetector : MonoBehaviour
{
    [SerializeField] protected PlayerHealth PlayerHealth;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private PlayerInfo _playerInfo;

    public bool IsMine => _playerInfo.IsMine;
    public bool IsBot => _playerInfo.IsBot;
    public PhotonView PhotonView => _photonView;

    public abstract void DetectHit(float damage, Player player, Vector3 targetPostition);

    public bool IsSameRootTransform(Transform otherRootTransform)
    {
        return otherRootTransform.Equals(_playerInfo.transform);
    }
}
