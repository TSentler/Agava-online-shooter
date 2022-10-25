using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grenade : MonoBehaviour
{
    [SerializeField] private float _throwForce;
    [SerializeField] private float _throwUpForce;

    private Rigidbody _rigidbody;

    public void Instantiate(Camera camera)
    {
        _rigidbody = GetComponent<Rigidbody>();

        Vector3 forceToAdd = camera.transform.forward * _throwForce + transform.up * _throwUpForce;

        _rigidbody.AddForce(forceToAdd, ForceMode.Impulse);
    }
}
