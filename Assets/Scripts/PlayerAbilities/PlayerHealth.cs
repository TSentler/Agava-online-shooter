using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace PlayerAbilities
{
    public class PlayerHealth : MonoBehaviour, IPunObservable
    {
        [SerializeField] private float _health;
        [SerializeField] private PhotonView _photonView;

        private int _kills;
        private int _deaths;
        private PlayerSpawner _spawner;

        public int Deaths => _deaths;
        private int Kills => _kills;
        public float Health => _health;

        public UnityAction<float> ChangeHealth;

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

        }

        private void Awake()
        {
            _deaths = 0;
            _kills = 0;
            PhotonNetwork.SetPlayerCustomProperties(new Hashtable() { { "Death", _deaths } });
            PhotonNetwork.SetPlayerCustomProperties(new Hashtable() { { "Kills", _kills } });
            _spawner = FindObjectOfType<PlayerSpawner>();
        }

        public void ApplyDamage(float damage, Player player)
        {
            object[] rpcParametrs = new object[2] { damage, player};
            _photonView.RPC(nameof(ApplyDamageRPC), RpcTarget.All, rpcParametrs);
        }

        [PunRPC]
        private void ApplyDamageRPC(float damage, Player player)
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
                _health -= damage;
                ChangeHealth?.Invoke(_health);

                if (_health <= 0)
                {
                    _deaths++;
                    PhotonNetwork.SetPlayerCustomProperties(new Hashtable() { { "Death", _deaths } });
                    int kills = (int)player.CustomProperties["Kills"] + 1;
                    Debug.Log(kills);
                    player.SetCustomProperties(new Hashtable() { { "Kills", kills } });
                    _spawner.SpawnPlayer();
                    //gameObject.SetActive(false);
                    PhotonNetwork.Destroy(gameObject);
                    Debug.Log("Устрой дестрой");
                }
            }
        }
    }
}