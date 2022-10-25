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
            foreach(var player in _players)
            {
                player.ApplyDamage(_explouseDamage, PhotonNetwork.LocalPlayer);
            }

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
