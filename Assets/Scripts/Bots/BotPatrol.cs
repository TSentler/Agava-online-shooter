using PlayerAbilities;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Bots
{
    [RequireComponent(typeof(BotMovement))]
    public class BotPatrol : MonoBehaviour
    {
        private NavTargetPoint[] _targets;
        private int _currentTargetPoint;

        public Vector3 TargetPointPosition =>
            _targets[_currentTargetPoint].transform.position;
            
        private void Awake()
        {
            _targets = FindObjectsOfType<NavTargetPoint>();
        }

        public void NextTarget()
        {
            var oldTarget = _currentTargetPoint;
            while (_targets.Length > 1 && oldTarget == _currentTargetPoint)
            {
                _currentTargetPoint = Random.Range(0, _targets.Length);
            }
        }
    }
}
