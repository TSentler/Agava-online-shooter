using System;
using Photon.Pun;
using UnityEngine;

namespace Network
{
    [RequireComponent(typeof(PhotonView))]
    public class LayerSetter : MonoBehaviour
    {
        [SerializeField] private LayerMask _mineLayer;
        
        private PhotonView _photonView;
        
        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            
            if (_photonView.IsMine)
            {
                var layer = LayermaskToLayer(_mineLayer); 
                gameObject.layer = layer;
                var childs = gameObject.
                    GetComponentsInChildren<Transform>(includeInactive: true);
                foreach (var child in childs)
                {
                    child.gameObject.layer = layer;
                }
            }
        }
        
        public int LayermaskToLayer(LayerMask layerMask) 
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
