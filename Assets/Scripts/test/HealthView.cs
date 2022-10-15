using Photon.Pun;
using PlayerAbilities;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class HealthView : MonoBehaviour, IPunObservable
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private PhotonView _photonView;

    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();

        if (_photonView.IsMine)
        {
            _text.text = _playerHealth.Health.ToString();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        _playerHealth.ChangeHealth += OnHealthChanged;
    }

    private void OnDisable()
    {
        _playerHealth.ChangeHealth -= OnHealthChanged;
    }

    private void OnHealthChanged(float health)
    {
        if (_photonView.IsMine)
        {
            _text.text = health.ToString();
        }    
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }
}
