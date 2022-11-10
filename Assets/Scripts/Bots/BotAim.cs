using PlayerAbilities;
using UnityEngine;
using UnityEngine.AI;

namespace Bots
{
    public class BotAim : MonoBehaviour
    {
        [SerializeField] private MouseLook _mouseLook;
        [Min(0f), SerializeField] private float _aimSpeed = 15f;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Transform _root;
        
        private float _tempAngularSpeed;

        private void Awake()
        {
            _tempAngularSpeed = _agent.angularSpeed;
            // _agent.desiredVelocity
        }

        public void StopAgentRotate()
        {
            _agent.angularSpeed = 0f;
        }
        
        public void ResetAgentRotate()
        {
            _agent.angularSpeed = _tempAngularSpeed;
        }

        public void Aim(Vector3 target)
        {
            var lookDirection = target - _root.position;
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
    }
}
