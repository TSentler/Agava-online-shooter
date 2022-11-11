using Photon.Realtime;
using UnityEngine;

public class BodyHit : HitDetector
{
    public override void DetectHit(float damage, Player player, Vector3 targetPostition)
    {
        PlayerHealth.ApplyDamage(damage, player, targetPostition);
    }
}
