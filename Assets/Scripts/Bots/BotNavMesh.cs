using System;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    public class BotNavMesh : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _target = FindObjectOfType<NavTargetPoint>().transform;
        }

        private void Update()
        {
            _agent.destination = _target.position;
        }
    }
}
