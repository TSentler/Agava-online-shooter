using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerAbilities
{
    public class PlayerHealth : MonoBehaviour, IPunObservable
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private PhotonView _photonView;
       // [SerializeField] private DamagebleHit _damagebleHit;
        [SerializeField] private WeaponsHolder _weaponsHolder;
        [SerializeField] private GameObject _player;

        private Animator _animator;
        private int _kills;
        private int _deaths;
        private float _currentHealth;
        private PlayerSpawner _spawner;
        private PlayerInfo _playerInfo;

        public UnityAction<float, float> ChangeHealth;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

        }

        public void EnableObject()
        {
            _photonView.RPC(nameof(EnableObjectRPC), RpcTarget.AllBuffered);
        }

        public void DisableObject()
        {
            
        }

        private void Awake()
        {
            _deaths = 0;
            _kills = 0;
            PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Death", _deaths } });
            PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Kills", _kills } });
            _spawner = FindObjectOfType<PlayerSpawner>();
            //_damagebleHit = FindObjectOfType<DamagebleHit>(true);
            //_damagebleHit.gameObject.SetActive(false);
            _playerInfo = GetComponent<PlayerInfo>();
        }

        private void OnEnable()
        {
            _currentHealth = _maxHealth;
            ChangeHealth?.Invoke(_currentHealth, _maxHealth);
           // _damagebleHit.gameObject.SetActive(false);
            _animator = _player.GetComponent<Animator>();
            _animator.SetBool("IsDie", false);
        }

        public bool NeedHeal()
        {
            return _currentHealth < _maxHealth;
        }

        public void TakeHeal(float heal)
        {
            if (_photonView.IsMine == false)
            {
                return;
            }

            if (NeedHeal())
            {
                if (_currentHealth + heal >= _maxHealth)
                {
                    _currentHealth = _maxHealth;
                }
                else
                {
                    _currentHealth += heal;
                }

                ChangeHealth?.Invoke(_currentHealth, _maxHealth);
            }
        }

        public void ApplyDamage(float damage, Player player, Vector3 targetPosition)
        {
            object[] rpcParametrs = new object[3] { damage, player, targetPosition };
            _photonView.RPC(nameof(ApplyDamageRPC), RpcTarget.All, rpcParametrs);
        }

        [PunRPC]
        private void ApplyDamageRPC(float damage, Player player, Vector3 targetPosition)
        {
            if (_photonView.IsMine == false)
            {
                return;
            }

            if (damage < 0)
            {
                throw new ArgumentOutOfRangeException("Damage can't be negative");
            }

            if (_photonView.IsMine)
            {
                _currentHealth -= damage;
                ChangeHealth?.Invoke(_currentHealth, _maxHealth);
                if (_playerInfo.IsBot == false)
                {
                    //_damagebleHit.gameObject.SetActive(true);
                    //_damagebleHit.ShowHitPoint(targetPosition, transform);
                    //StartCoroutine(DestroyEffectWithDelay());
                }

                if (_currentHealth <= 0)
                {
                    _deaths++;
                    if (_playerInfo.IsBot == false)
                    {
                        int deathes = (int)player.CustomProperties["Death"] + 1;
                        PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Death", deathes } });
                        int kills = (int)player.CustomProperties["Kills"] + 1;
                        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Kills", kills } });
                    }

                    _animator = _player.GetComponent<Animator>();
                    //_animator.SetTrigger("Die");
                    _animator.SetBool("IsDie", true);
                    StartCoroutine(DisableWithDelay());
                    //_spawner.SpawnPlayer(this);
                    _weaponsHolder.SetNewGun(0);
                }
            }
        }

        [PunRPC]
        private void DisableObjectRPC()
        {
            _spawner.SpawnPlayer(this);
            gameObject.SetActive(false);
        }

        [PunRPC]
        private void EnableObjectRPC()
        {
            gameObject.SetActive(true);
        }

        //private IEnumerator DestroyEffectWithDelay()
        //{
        //    yield return new WaitForSeconds(10f);
        //    _damagebleHit.gameObject.SetActive(false);
        //}

        private IEnumerator DisableWithDelay()
        {
            yield return new WaitForSeconds(3f);
            _photonView.RPC(nameof(DisableObjectRPC), RpcTarget.AllBuffered);
        }
    }
}