using System;
using CharacterInput;
using PlayerAbilities;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Bots
{
    public class BotNavMesh : MonoBehaviour, ICharacterInputSource
    {
        [SerializeField] private MouseLook _mouseLook;
        [Min(0f), SerializeField] private float _aimSpeed = 5f;
        [SerializeField] private PlayerHand _playerHand;
        
        private NavMeshAgent _agent;
        private NavTargetPoint[] _targets;
        private float _tempAngularSpeed;
        private int _currentTargetPoint;
        private bool _isStop;

        public Vector2 MovementInput { get; private set; }
        public bool IsJumpInput { get; private set; }
        public bool IsLeftMouseButtonDown { get; private set; }
        public bool IsLeftMouseButton { get; private set; }

        private Vector3 TargetPointPosition =>
            _targets[_currentTargetPoint].transform.position;
            
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _targets = FindObjectsOfType<NavTargetPoint>();
            _tempAngularSpeed = _agent.angularSpeed;
        }

        private void Update()
        {
            var rightInput = GetInputByAxis(transform.right);
            var forwardInput = GetInputByAxis(transform.forward);
            MovementInput = new Vector2(rightInput, forwardInput);
        }

        public void GoDestination()
        {
            if (_isStop)
            {
                // ReturnAgentRotate();
            }
            _isStop = false;
            _agent.isStopped = false;
            _agent.destination = TargetPointPosition;
        }

        public void Stop()
        {
            if (_isStop)
                return;
            
            _agent.destination = transform.position;
            _agent.isStopped = true;
            StopAgentRotate();
            _isStop = true;
        }

        public void StopAgentRotate()
        {
            _agent.angularSpeed = 0f;
        }
        
        public void ResetAgentRotate()
        {
            _agent.angularSpeed = _tempAngularSpeed;
        }

        public void Aim(GameObject target)
        {
            var lookDirection = target.transform.position - 
                                transform.position;
            var lookDirectionXZ = lookDirection;
            lookDirectionXZ.y = 0f;
            VerticalAim(lookDirectionXZ, lookDirection);
            HorizontalAim(lookDirectionXZ);
            StopAgentRotate();
        }

        public void ResetVerticalAim()
        {
            VerticalAim(transform.forward, transform.forward);
        }

        private void VerticalAim(Vector3 lookDirectionXZ, Vector3 lookDirection)
        {
            var currentYDirection =
                Quaternion.AngleAxis(_mouseLook.XRotation, transform.right)
                * lookDirectionXZ;
            var yRotation = Vector3.SignedAngle(currentYDirection,
                lookDirection, transform.right);
            // Debug.DrawRay(transform.position, currentYDirection, Color.blue);
            // Debug.DrawRay(transform.position, lookDirection, Color.green);
            // Debug.DrawRay(transform.position, lookDirectionXZ, Color.red);
            var yAngle = Vector3.SignedAngle(lookDirectionXZ,
                lookDirection, transform.right);
            yRotation = Mathf.Lerp(0f, yRotation, _aimSpeed * Time.deltaTime);
            _mouseLook.MouseMove(0f, -yRotation);
        }

        private void HorizontalAim(Vector3 lookDirectionXZ)
        {
            var lookRotation = Quaternion.LookRotation(lookDirectionXZ);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                lookRotation, _aimSpeed * Time.deltaTime);
        }

        public void NextTarget()
        {
            var oldTarget = _currentTargetPoint;
            while (_targets.Length > 1 && oldTarget == _currentTargetPoint)
            {
                _currentTargetPoint = Random.Range(0, _targets.Length);
            }
        }

        public void SetDestinationTarget()
        {
            
        }

        public void Shoot(Vector3 direction)
        {
            var ray = new Ray(transform.position, direction);
            _playerHand.CurentGun.Shoot(ray, transform);
        }
        
        private float GetInputByAxis(Vector3 axis)
        {
            var input = Vector3.Dot(_agent.velocity, axis);
            return Mathf.Clamp(input, -1f, 1f);
        }
    }
}
