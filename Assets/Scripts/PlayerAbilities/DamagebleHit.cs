using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagebleHit : MonoBehaviour
{
    [SerializeField] private Transform _hitPoint;

    private Vector3 _direction;

    public void ShowHitPoint(Vector3 enemyPosition, Vector3 playerPosition, Vector3 playerForward)
    {
        _direction = playerPosition - enemyPosition;
        var angle = Vector3.SignedAngle(playerForward, _direction, Vector3.up) * -1;
        _hitPoint.gameObject.SetActive(true);
        _hitPoint.localRotation = Quaternion.Euler(angle * Vector3.forward);
    }
}
