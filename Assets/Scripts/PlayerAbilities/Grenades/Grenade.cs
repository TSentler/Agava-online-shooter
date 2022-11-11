using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerAbilities
{
    [RequireComponent(typeof(Rigidbody))]
    public class Grenade : MonoBehaviour
    {
        [SerializeField] private float _throwForce;
        [SerializeField] private float _throwUpForce;
        [SerializeField] private float _explouseCooldown;
        [SerializeField] private int _explouseDamage;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private AudioSource _explousSound;
        [SerializeField] private MeshRenderer[] _renderers;
        [SerializeField] private PhotonView _photonView;

        private Rigidbody _rigidbody;
        private bool _isEplouseCooldownStart;
        private float _currentTimer;
        private List<PlayerHealth> _players = new List<PlayerHealth>();

        private void Update()
        {
            if (_isEplouseCooldownStart == false)
                return;
            _currentTimer += Time.deltaTime;

            if (_currentTimer >= _explouseCooldown)
            {
                Explouse();
            }
        }

        public void Instantiate(Camera camera)
        {
            _rigidbody = GetComponent<Rigidbody>();
            Vector3 forceToAdd = camera.transform.forward * _throwForce + transform.up * _throwUpForce;
            _rigidbody.AddForce(forceToAdd, ForceMode.Impulse);
            _isEplouseCooldownStart = true;
        }

        private void Explouse()
        {
            _isEplouseCooldownStart = false;
            _photonView.RPC(nameof(EnableEffectRPC), RpcTarget.All);

            foreach(var player in _players)
            {
                player.ApplyDamage(_explouseDamage, PhotonNetwork.LocalPlayer, transform.position);
            }

            StartCoroutine(DestroyWihDelay());
        }

        [PunRPC]
        private void EnableEffectRPC()
        {
            _particle.Play();
            _explousSound.Play();

            foreach (var renderer in _renderers)
                renderer.enabled = false;
        }

        private IEnumerator DestroyWihDelay()
        {
            yield return new WaitForSeconds(0.5f);
            PhotonNetwork.Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                _players.Add(playerHealth);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                _players.Remove(playerHealth);
            }
        }
    }
}
