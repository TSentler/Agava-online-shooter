using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(PhotonView))]
public class ItemPresenter : MonoBehaviour
{
    [SerializeField] protected Transform ShowPoint;
    [SerializeField] protected float TimerToSpawn;

    protected bool CanUse = true;
    protected PhotonView PhotonView;
    protected SphereCollider Collider;

    private Vector3 _rotation = new Vector3(0, 40, 0);

    private void Start()
    {
        Collider = GetComponent<SphereCollider>();
        PhotonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        ShowPoint.Rotate(_rotation * Time.deltaTime);
    }
}
