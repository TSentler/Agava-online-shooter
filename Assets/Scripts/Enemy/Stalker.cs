using UnityEngine;

namespace Enemy
{
    public class Stalker : MonoBehaviour
    {
        private StalkerTarget[] _targets;
        private StalkerTarget _target;

        public bool HasTarget => _target != false;
        public Vector3 Position => _target.transform.position;

        
        private void Start()
        {
            _targets = FindObjectsOfType<StalkerTarget>();
        }
        
        private float GetDistance(Vector3 position)
        {
            return Vector3.Distance(position, transform.position);
        }

        public float GetDistance()
        {
            return GetDistance(Position);
        }

        public bool TrySelectTarget()
        {
            StalkerTarget selected = null;
            var minDistance = float.MaxValue;
            for (int i = 0; i < _targets.Length; i++)
            {
                var target = _targets[i];
                if (target == null)
                    continue;

                var distance = GetDistance(target.transform.position);
                if (distance < minDistance)
                {
                    selected = target;
                    minDistance = distance;
                }
            }

            _target = selected ?? _target;
            return selected != null;
        }

        public void Contact()
        {
            _target.Contact(); 
        }
    }
}
