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
            // _agent.desiredVelocity
        }

        private void Update()
        {
            var rightInput = GetInputByAxis(transform.right);
            var forwardInput = GetInputByAxis(transform.forward);
            MovementInput = new Vector2(rightInput, forwardInput);
        }

        private float GetInputByAxis(Vector3 axis)
        {
            var input = Vector3.Dot(_agent.velocity, axis);
            return Mathf.Clamp(input, -1f, 1f);
        }
    }
}
