using System;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    public class BotMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;

        public void GoDestination(Vector3 destination)
        {
            _agent.isStopped = false;
            _agent.SetDestination(destination);
        }

        public void Stop()
        {
            if (_agent.isStopped)
                return;

            _agent.SetDestination(transform.position);
            _agent.isStopped = true;
        }

    }
}
