using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDisabler : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;

    private void Awake()
    {
        if (_photonView.IsMine == false)
        {
            var cameras = GetComponentsInChildren<Camera>();

            foreach(var camera in cameras)
            {
                camera.enabled = false;
            }
        }
    }
}
