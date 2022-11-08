using CharacterInput;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    public class BotInput : MonoBehaviour, ICharacterInputSource
    {
        [SerializeField] private NavMeshAgent _agent;
        
        public Vector2 MovementInput { get; private set; }
        public bool IsJumpInput { get; private set; }

        private void Awake()
        {
            _agent.updatePosition = false;
        }

        private void Update()
        {
            Debug.Log(_agent.desiredVelocity);
            //Debug.Log(_agent.steeringTarget);
            var rightInput = GetInputByAxis(transform.right);
            var forwardInput = GetInputByAxis(transform.forward);
            MovementInput = new Vector2(rightInput, forwardInput);
            _agent.nextPosition = transform.position;
        }

        private float GetInputByAxis(Vector3 axis)
        {
            var input = Vector3.Dot(_agent.desiredVelocity, axis);
            return Mathf.Clamp(input, -1f, 1f);
        }
    }
}
