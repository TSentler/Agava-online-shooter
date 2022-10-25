using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyHit : HitDetector
{
    public override void DetectHit(float damage, Player player)
    {
        PlayerHealth.ApplyDamage(damage, player);
        Debug.Log("Туловище");
    }
}
