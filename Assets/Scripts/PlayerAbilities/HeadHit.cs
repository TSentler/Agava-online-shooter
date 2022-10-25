using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadHit : HitDetector
{
    public override void DetectHit(float damage, Player player)
    {
        PlayerHealth.ApplyDamage(damage * 2, player);
        Debug.Log("Голова");
    }
}
