using System;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(PhotonView))]
    public class LayerSetter : MonoBehaviour
    {
        [SerializeField] private LayerMask _mineLayer;
        [SerializeField] private GameObject[] _settable;
        [SerializeField] private Transform[] _withChilds;
        
        private PhotonView _photonView;
        
        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            
            if (_photonView.IsMine)
            {
                var layer = LayermaskToLayer(_mineLayer); 
                foreach (var child in _settable)
                {
                    child.layer = layer;
                }

                for (int i = 0; i < _withChilds.Length; i++)
                {
                    foreach (Transform child in
                             _withChilds[i].GetComponentsInChildren<Transform>())
                    {
                        child.gameObject.layer = layer;
                    }
                }
            }
        }
        
        private int LayermaskToLayer(LayerMask layerMask) 
        {
            int layerNumber = 0;
            int layer = layerMask.value;
            while(layer > 0) 
            {
                layer = layer >> 1;
                layerNumber++;
            }
            return layerNumber - 1;
        }
    }
}
