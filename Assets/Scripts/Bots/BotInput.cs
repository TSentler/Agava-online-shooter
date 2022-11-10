using CharacterInput;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    public class BotInput : MonoBehaviour, ICharacterInputSource
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _angularSpeed = 150f, _rotationTime = 20f;
        [SerializeField] private float _horizontalAngleDeadZone = 5f;

        public Vector2 MovementInput { get; private set; }
        public Vector2 MouseInput { get; private set; }
        public bool IsJumpInput { get; private set; }

        private void Awake()
        {
            _agent.updatePosition = false;
            //_agent.updateRotation = false;
        }
        
        private void Update()
        {
            SetMovementInput();
            //SetMouseInput();
        }

        private void SetMouseInput()
        {
            var x = CalculateMouseX(_agent.steeringTarget);
            x = Mathf.Lerp(0f, x, _rotationTime * Time.deltaTime);

            // Debug.DrawLine(transform.position, _agent.steeringTarget, Color.yellow);
            // Debug.DrawRay(transform.position, forwardXZ, Color.blue);
            // Debug.DrawRay(transform.position, dirXZ, Color.magenta);

            var y = CalculateMouseY(_agent.steeringTarget);
            y = Mathf.Lerp(0f, y, _rotationTime * Time.deltaTime);
            
            MouseInput = new Vector2(x * _angularSpeed, 0f);
        }

        private void SetMovementInput()
        {
            var rightInput = GetInputByAxis(transform.right);
            var forwardInput = GetInputByAxis(transform.forward);
            MovementInput = new Vector2(rightInput, forwardInput);
            _agent.nextPosition = transform.position;
        }
        
        private float CalculateMouseX(Vector3 targetPosition)
        {
            var dir = targetPosition - transform.position;
            var angle = Vector3.SignedAngle(transform.forward,
                dir, Vector3.up);
            if (Mathf.Abs(angle) < _horizontalAngleDeadZone)
                return 0f;
            
            var x = angle / 180f;
            return x;
        }

        private float CalculateMouseY(Vector3 targetPosition)
        {
            var dir = targetPosition - transform.position;
            var angle = Vector3.SignedAngle(transform.forward,
                dir, Vector3.right);
            if (Mathf.Abs(angle) < _horizontalAngleDeadZone)
                return 0f;
            
            var y = angle / 180f;
            return y;
        }

        private float GetInputByAxis(Vector3 axis)
        {
            var input = Vector3.Dot(_agent.desiredVelocity, axis);
            return Mathf.Clamp(input, -1f, 1f);
        }
    }
}
