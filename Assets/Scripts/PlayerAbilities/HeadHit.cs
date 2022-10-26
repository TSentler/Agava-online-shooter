using Photon.Realtime;

public class HeadHit : HitDetector
{
    public override void DetectHit(float damage, Player player)
    {
        PlayerHealth.ApplyDamage(damage * 1.5f, player);
    }
}
