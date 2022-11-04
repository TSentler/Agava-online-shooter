using System;
using BehaviorDesigner.Runtime;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    [RequireComponent(typeof(PhotonView), 
        typeof(NavMeshAgent), 
        typeof(BehaviorTree))]
    public class BotSynchronizer : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private BehaviorTree _behavior;
        private PhotonView _photonView;

        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _behavior = GetComponent<BehaviorTree>();
            _agent = GetComponent<NavMeshAgent>();
            if (_photonView.IsMine == false)
            {
                _agent.enabled = false;
                _behavior.enabled = false;
            }
        }
    }
}
