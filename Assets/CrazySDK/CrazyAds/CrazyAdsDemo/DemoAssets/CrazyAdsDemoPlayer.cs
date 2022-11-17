using UnityEngine;
using CrazyGames;

[RequireComponent(typeof(Rigidbody))]
public class CrazyAdsDemoPlayer : MonoBehaviour
{
    [SerializeField] private CrazyAdType adType = CrazyAdType.midgame;

    private readonly Vector3 pushForce = Vector3.right * 2;
    private Rigidbody rb;
    private Vector3 startPos;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }


    private void FixedUpdate()
    {
        transform.Translate(pushForce * Time.fixedDeltaTime);

        if (!(transform.position.y < -20)) return;
        print("Player Died!  Starting Ad Break!");
        if (adType == CrazyAdType.rewarded) CrazyAds.Instance.beginAdBreakRewarded(respawn);
        else CrazyAds.Instance.beginAdBreak(respawn);
    }


    private void respawn()
    {
        print("Ad Finished!  So respawning player!");

        transform.position = startPos;
        rb.velocity = Vector3.zero;
    }
}