using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

[RequireComponent(typeof(Animator))]
public class PlayerHandler : MonoBehaviour, IPunObservable
{
    [SerializeField] private float _directionalDampTime = 0.25f;

    private Animator _animator;
    private PhotonView _photonView;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
     
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _photonView = FindObjectOfType<PhotonView>();

    }

    private void Update()
    {
        //if (_photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        //{
        //    return;
        //}

        float axisHorizontal = Input.GetAxis("Horizontal");
        float axisVertical = Input.GetAxis("Vertical");

        if(axisVertical < 0)
        {
            axisVertical = 0;
        }
        //Vector3 movementVector = new Vector3(axisHorizontal, 0f, axisVertical);
        //transform.Translate(movementVector * _speed * Time.deltaTime);

        _animator.SetFloat("Speed", axisVertical * axisHorizontal + axisVertical * axisVertical);
        _animator.SetFloat("Direction", axisHorizontal, _directionalDampTime, Time.deltaTime);
    }
}
