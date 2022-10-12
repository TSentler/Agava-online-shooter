using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    //[SerializeField] private GameObject _playerPrefab;

    private void Start()
    {
        PhotonNetwork.Instantiate("Robot Kyle", transform.position,Quaternion.identity,0);
    }
}
