using System;
using CharacterInput;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Bots
{
    public class BotNavMesh : MonoBehaviour, ICharacterInputSource
    {
        private NavMeshAgent _agent;
        private NavTargetPoint[] _targets;
        private int _currentTarget;

        public Vector2 MovementInput { get; private set; }
        public bool IsJumpInput { get; private set; }
        
        private Vector3 TargetPosition =>
            _targets[_currentTarget].transform.position;
            
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _targets = FindObjectsOfType<NavTargetPoint>();
        }

        private void Update()
        {
            var rightInput = GetInputByAxis(transform.right);
            var forwardInput = GetInputByAxis(transform.forward);
            MovementInput = new Vector2(rightInput, forwardInput);
        }

        public void GoDestination()
        {
            _agent.destination = TargetPosition;
        }

        public void Stop()
        {
            _agent.destination = transform.position;
        }

        public void NextTarget()
        {
            var oldTarget = _currentTarget;
            while (_targets.Length > 1 && oldTarget == _currentTarget)
            {
                _currentTarget = Random.Range(0, _targets.Length);
            }
        }

        private float GetInputByAxis(Vector3 axis)
        {
            var input = Vector3.Dot(_agent.velocity, axis);
            return Mathf.Clamp(input, -1f, 1f);
        }
    }
}
