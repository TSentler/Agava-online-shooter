using System;
using BehaviorDesigner.Runtime;
using Photon.Pun;
using PlayerAbilities;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    [RequireComponent(typeof(PlayerInfo), 
        typeof(NavMeshAgent), 
        typeof(BehaviorTree))]
    public class BotSynchronizer : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private BehaviorTree _behavior;
        private PlayerInfo _playerInfo;

        private void Awake()
        {
            _playerInfo = GetComponent<PlayerInfo>();
            _behavior = GetComponent<BehaviorTree>();
            _agent = GetComponent<NavMeshAgent>();
            if (_playerInfo.IsBot == false)
                return;
            
            if (_playerInfo.IsMine == false)
            {
                _agent.enabled = false;
                _behavior.enabled = false;
            }
        }
    }
}
