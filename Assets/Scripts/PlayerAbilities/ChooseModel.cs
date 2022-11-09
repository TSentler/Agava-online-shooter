using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseModel : MonoBehaviour
{
    [SerializeField] private GameObject _mineModel;
    [SerializeField] private GameObject _notMineModel;
    [SerializeField] private PhotonView _photonView;

    private void Awake()
    {
        if (_photonView.IsMine)
        {
            _mineModel.SetActive(true);
            _notMineModel.SetActive(false);
        }
        else
        {
            _mineModel.SetActive(false);
            _notMineModel.SetActive(true);
        }
    }
}
