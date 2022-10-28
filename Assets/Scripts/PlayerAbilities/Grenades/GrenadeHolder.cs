using System;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using PlayerAbilities.AnimationBehaviours;
using UnityEngine;

namespace PlayerAbilities
{
    public class GrenadeHolder : MonoBehaviour
    {
        private readonly int _throwName = Animator.StringToHash("Throw");
        
        [SerializeField] private int _maxGrenades;
        [SerializeField] private Grenade _grenade;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private Animator _animator;
        
        private int _currentGrenade;

        private void Awake()
        {
            _currentGrenade = _maxGrenades;
        }

        private void Update()
        {
            if (_photonView.IsMine)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    ThrowGrenade();
                    _currentGrenade--;
                }
            }          
        }

        private void ThrowGrenade()
        {
            if (_currentGrenade <= 0)
                return;

            _animator.SetTrigger(_throwName);
        }

        public void SpawnGrenade()
        {
            GameObject grenade = PhotonNetwork.Instantiate(_grenade.name, _spawnPoint.position, Quaternion.identity);
            grenade.GetComponent<Grenade>().Instantiate(_camera);
        }
    }
}
