using UnityEngine;

namespace Enemy.Player
{
    [RequireComponent(typeof(Health))]
    public class PlayerStalkerTarget : StalkerTarget
    {
        private Health _health;

        private void Awake()
        {
            _health = GetComponent<Health>();
        }

        public override void Contact()
        {
            _health.TakeDamage();
        }
    }
}
