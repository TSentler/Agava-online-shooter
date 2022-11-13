using UnityEngine;

public class bl_SimplePlayerController : MonoBehaviour
{  
    private Vector3 LastImpactDirection = Vector3.forward;

    void OnEnable()
    {
        bl_DamageDelegate.OnIndicator += OnImpact;
    }

    void OnDisable()
    {
        bl_DamageDelegate.OnIndicator -= OnImpact;
    }

    void OnImpact(bl_IndicatorInfo info)
    {
        LastImpactDirection = info.Direction;
    }  
}