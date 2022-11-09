using PlayerAbilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Bots
{
    public class BotShoot : MonoBehaviour
    {
        [SerializeField] private MouseLook _mouseLook;
        [SerializeField] private PlayerHand _playerHand;
        [SerializeField] private float _angleShootSpread = 20f;
        
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
    }
}
