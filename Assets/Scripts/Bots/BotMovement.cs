using System;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    public class BotMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;

        private bool _isStop;

        public void GoDestination(Vector3 destination)
        {
            _isStop = false;
            _agent.isStopped = false;
            _agent.destination = destination;
        }

        public void Stop()
        {
            if (_isStop)
                return;

            _agent.destination = transform.position;
            _agent.isStopped = true;
            _isStop = true;
        }
    }
}
