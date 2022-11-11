using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagebleHit : MonoBehaviour
{
    [SerializeField] private Transform _hitPoint;

    private Vector3 _direction;  

    public void ShowHitPoint(Vector3 targetPosition, Transform player)
    {
        _direction = player.position - targetPosition;
        Quaternion targetRoot = Quaternion.LookRotation(_direction);
        targetRoot.z = -targetRoot.y;
        targetRoot.x = 0;
        targetRoot.y = 0;
        Vector3 _northDirection = new Vector3(0, 0, -player.eulerAngles.y);
        _hitPoint.localRotation = targetRoot * Quaternion.Euler(_northDirection);
    }
}
