using Photon.Pun;
using UnityEngine;

public class PlaerSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;

    //public override void OnJoinedRoom()
    //{
    //    base.OnJoinedRoom();
    //    PhotonNetwork.Instantiate("Robot Kyle", new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
    //    Debug.Log("On join room");
    //}

    private void Start()
    {
        PhotonNetwork.Instantiate(_playerPrefab.name, 
            transform.position,Quaternion.identity,0);
    }

    //private void Awake()
    //{   
    //    if (CameraHandler.LocalPlayerInstance == null)
    //    {
    //        Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
    //        PhotonNetwork.Instantiate("Robot Kyle", new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
    //    }
    //    else
    //    {
    //        Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
    //    }
    //}

}
