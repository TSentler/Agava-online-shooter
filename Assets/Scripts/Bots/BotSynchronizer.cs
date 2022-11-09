using System;
using BehaviorDesigner.Runtime;
using Photon.Pun;
using PlayerAbilities;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    public class BotSynchronizer : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private PlayerInfo _playerInfo;
        [SerializeField] private BehaviorTree _behavior;

        private void Awake()
        {
            if (_playerInfo.IsBot && _playerInfo.IsMine == false)
            {
                _agent.enabled = false;
                _behavior.enabled = false;
            }
        }
    }
}
