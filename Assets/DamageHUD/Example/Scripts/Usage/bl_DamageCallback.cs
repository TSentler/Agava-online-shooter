using Photon.Pun;
using UnityEngine;

public class bl_DamageCallback : MonoBehaviour
{

    [SerializeField] private PhotonView _photonView;

    public PhotonView PhotonView => _photonView;

    public void OnDamage(bl_DamageInfo info)
    {
        if (PhotonView.IsMine)
        {
            bl_DamageDelegate.OnDamageEvent(info);
        }
    }
}