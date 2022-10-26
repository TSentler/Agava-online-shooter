using Photon.Realtime;

public class BodyHit : HitDetector
{
    public override void DetectHit(float damage, Player player)
    {
        PlayerHealth.ApplyDamage(damage, player);
    }
}
