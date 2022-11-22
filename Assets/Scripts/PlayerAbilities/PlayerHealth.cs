using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PlayerAbilities
{
    public class PlayerHealth : MonoBehaviour, IPunObservable
    {
        private readonly string _increaseHPName = "IncreaseHP";
        private const string ShotgunSaveKey = "ShotgunDontDrope";
        private const string RifleSaveKey = "RifleDontDrope";

        [SerializeField] private float _maxHealth;
        [SerializeField] private PhotonView _photonView;
        [SerializeField] private DamagebleHit _damagebleHit;
        [SerializeField] private WeaponsHolder _weaponsHolder;
        [SerializeField] private GameObject _player;
        [SerializeField] private DeadPanel _deadPanel;
        [SerializeField] private Camera[] _cameras;
        [SerializeField] private FPSControllerLPFP.FpsControllerLPFP _fpsController;
        [SerializeField] private HandgunScriptLPFP _handgunScript;
        [SerializeField] private PumpShotgunScriptLPFP _pumpShotgun;
        [SerializeField] private AutomaticGunScriptLPFP _automaticGunScript;
        [SerializeField] private BoltActionSniperScriptLPFP _sniperScript;
        [SerializeField] private int _respawnPrice;

        private Animator _animator;
        private int _kills;
        private int _deaths;
        private float _currentHealth;
        private PlayerSpawner _spawner;
        private PlayerInfo _playerInfo;
        private float _oldMaxHealth;
        private KillFidPanel _killFidPanel;
        private float _timer;
        private TMP_Text _text;
        private int _rifleByed;
        private int _shotgunByed;
        private RevardedMoneyHolder _revardedMoneyHolder;
        private Respawn _respawn;

        public PhotonView PhotonView => _photonView;

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
            _damagebleHit = FindObjectOfType<DamagebleHit>(true);
            _damagebleHit.gameObject.SetActive(false);
            _playerInfo = GetComponent<PlayerInfo>();
            _oldMaxHealth = _maxHealth;
            _killFidPanel = FindObjectOfType<KillFidPanel>();
            _deadPanel = FindObjectOfType<DeadPanel>();
            _deadPanel.gameObject.SetActive(true);
            _shotgunByed = PlayerPrefs.GetInt(ShotgunSaveKey, 0);
            _rifleByed = PlayerPrefs.GetInt(RifleSaveKey, 0);
            _revardedMoneyHolder = FindObjectOfType<RevardedMoneyHolder>();
            var fidPanel = _killFidPanel.gameObject.GetComponent<CanvasGroup>();
            fidPanel.alpha = 0f;
            _text = _deadPanel.Text;
            _respawn = FindObjectOfType<Respawn>();
            InitializeImprovements();
            _respawn.GetComponent<Button>().onClick.AddListener(Respawn);
            _animator = _player.GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _currentHealth = _maxHealth;
            ChangeHealth?.Invoke(_currentHealth, _maxHealth);
            _damagebleHit.gameObject.SetActive(false);
            _animator.SetBool("IsDie", false);

            if (_deadPanel == null)
            {
                Debug.LogWarning("DeadPanel is NULL");
                return;
            }

            foreach(var camera in _cameras)
            {
                camera.enabled = true;
            }

            _fpsController.enabled = true;
            _handgunScript.enabled = true;
            _automaticGunScript.enabled = true;
            _sniperScript.enabled = true;
            _deadPanel.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void OnDisable()
        {
            ResetImprovements();
        }

        private void ResetImprovements()
        {
            if (_photonView.IsMine)
            {
                _maxHealth = _oldMaxHealth;
            }
        }

        private void InitializeImprovements()
        {
            if (_photonView.IsMine && PlayerPrefs.HasKey(_increaseHPName))
            {
                _maxHealth += PlayerPrefs.GetFloat(_increaseHPName);
                PlayerPrefs.DeleteKey(_increaseHPName);
            }
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

        public void ApplyDamage(float damage, Player player, Vector3 enemy)
        {
            object[] rpcParametrs = new object[3] { damage, player, enemy};
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

            if (_photonView.IsMine && _currentHealth > 0f)
            {
                _currentHealth -= damage;
                ChangeHealth?.Invoke(_currentHealth, _maxHealth);
                if (_playerInfo.IsBot == false)
                {
                    _damagebleHit.gameObject.SetActive(true);
                    _damagebleHit.ShowHitPoint(targetPosition, transform.position, transform.forward);
                    StartCoroutine(DestroyEffectWithDelay());
                }

                if (_currentHealth <= 0)
                {
                    _deaths++;
                    if (_playerInfo.IsBot == false)
                    {
                        _killFidPanel.InstantiateKills(player.NickName, PhotonNetwork.NickName);
                        int deathes = (int)player.CustomProperties["Death"] + 1;
                        PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Death", deathes } });
                        int kills = (int)player.CustomProperties["Kills"] + 1;
                        player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Kills", kills } });
                    }

                    //_animator.SetTrigger("Die");
                    _animator.SetBool("IsDie", true);
                    _timer = 3f;
                    StartCoroutine(DisableWithDelay());
                    //_spawner.SpawnPlayer(this);
                    
                    
                    if(_rifleByed != 1 && _weaponsHolder.CurrentGunId != 1 || _shotgunByed != 1 && _weaponsHolder.CurrentGunId != 2)
                    {
                        _weaponsHolder.SetNewGun(0);
                    }

                    Cursor.lockState = CursorLockMode.None;
                    
                    foreach(var camera in _cameras)
                    {
                        camera.enabled = false;
                    }

                    _fpsController.enabled = false;
                    _handgunScript.enabled = false;
                    _automaticGunScript.enabled = false;
                    _sniperScript.enabled = false;
                    _deadPanel.gameObject.SetActive(true);
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

        private IEnumerator DestroyEffectWithDelay()
        {
            yield return new WaitForSeconds(3f);
            _damagebleHit.gameObject.SetActive(false);
        }

        private IEnumerator DisableWithDelay()
        {
            _text.text = _timer.ToString("0") ;
            Debug.Log(_timer);
            _timer -= Time.deltaTime;
            yield return new WaitForSeconds(3f);
            _photonView.RPC(nameof(DisableObjectRPC), RpcTarget.AllBuffered);
        }

        public void Respawn()
        {
            if (_revardedMoneyHolder.Money >= _respawnPrice)
            {
                StopCoroutine(DisableWithDelay());
                _photonView.RPC(nameof(DisableObjectRPC), RpcTarget.All);
                _photonView.RPC(nameof(EnableObject), RpcTarget.All);
                _revardedMoneyHolder.TakeMoney(_respawnPrice);
            }
        }
    }
}