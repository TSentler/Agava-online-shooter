using Photon.Realtime;
using UnityEngine;

public class HeadHit : HitDetector
{
    public override void DetectHit(float damage, Player player, Vector3 targetPostition)
    {
        PlayerHealth.ApplyDamage(damage * 1.5f, player, targetPostition);
    }

    public override float GetCalculatedDamage(float damage)
    {
        return damage * 1.5f;
    }
}
