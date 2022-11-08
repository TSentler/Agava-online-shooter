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
        [SerializeField] private float _angleShootSpread = -20f;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Transform _root;
        
        private NavTargetPoint[] _targets;
        private float _tempAngularSpeed;
        private int _currentTargetPoint;
        private bool _isStop;

        public Vector2 MovementInput { get; private set; }
        public bool IsJumpInput { get; private set; }

        private Vector3 TargetPointPosition =>
            _targets[_currentTargetPoint].transform.position;
            
        private void Awake()
        {
            _targets = FindObjectsOfType<NavTargetPoint>();
            _tempAngularSpeed = _agent.angularSpeed;
            // _agent.desiredVelocity
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
                                _root.position;
            var lookDirectionXZ = lookDirection;
            lookDirectionXZ.y = 0f;
            VerticalAim(lookDirectionXZ, lookDirection);
            HorizontalAim(lookDirectionXZ);
            StopAgentRotate();
        }

        public void ResetVerticalAim()
        {
            VerticalAim(_root.forward, _root.forward);
        }

        private void VerticalAim(Vector3 lookDirectionXZ, Vector3 lookDirection)
        {
            var currentYDirection =
                Quaternion.AngleAxis(_mouseLook.XRotation, _root.right)
                * lookDirectionXZ;
            var yRotation = Vector3.SignedAngle(currentYDirection,
                lookDirection, _root.right);
            // Debug.DrawRay(transform.position, currentYDirection, Color.blue);
            // Debug.DrawRay(transform.position, lookDirection, Color.green);
            // Debug.DrawRay(transform.position, lookDirectionXZ, Color.red);
            var yAngle = Vector3.SignedAngle(lookDirectionXZ,
                lookDirection, _root.right);
            yRotation = Mathf.Lerp(0f, yRotation, _aimSpeed * Time.deltaTime);
            _mouseLook.MouseMove(0f, -yRotation);
        }

        private void HorizontalAim(Vector3 lookDirectionXZ)
        {
            var lookRotation = Quaternion.LookRotation(lookDirectionXZ);
            _root.rotation = Quaternion.Slerp(_root.rotation,
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

        public void Shoot(Transform target)
        {
            // target.GetComponent<PlayerInfo>();
            var shootPosition = _mouseLook.transform.position;
            var targetAim = target.position + new Vector3(0f, 1f, 0f);
            var direction = targetAim - shootPosition;
            direction = AddSpread(direction);
            var gun = _playerHand.CurentGun;
            var ray = new Ray(shootPosition, direction);
            Debug.DrawRay(shootPosition, direction, Color.blue, 1f);
            
            gun.Shoot(ray, _mouseLook.transform);
        }

        private Vector3 AddSpread(Vector3 direction)
        {
            var spreadAngle = Quaternion.AngleAxis(
                Random.Range(-_angleShootSpread, _angleShootSpread),
                _mouseLook.transform.right);
            direction = spreadAngle * direction;
            spreadAngle = Quaternion.AngleAxis(
                Random.Range(-_angleShootSpread, _angleShootSpread),
                _mouseLook.transform.forward);
            direction = spreadAngle * direction;
            return direction;
        }

        private float GetInputByAxis(Vector3 axis)
        {
            var input = Vector3.Dot(_agent.velocity, axis);
            return Mathf.Clamp(input, -1f, 1f);
        }
    }
}
