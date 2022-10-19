using Photon.Pun;
using PlayerAbilities;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour, IPunObservable
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Slider _slider;
    [SerializeField] private PhotonView _photonView;

    private void Awake()
    {
        if (_photonView.IsMine)
        {
            _slider.value = 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        _slider.value = 1;
        _playerHealth.ChangeHealth += OnHealthChanged;
    }

    private void OnDisable()
    {
        _playerHealth.ChangeHealth -= OnHealthChanged;
    }

    private void OnHealthChanged(float currentHealth, float maxHealth)
    {
        if (_photonView.IsMine)
        {
            _slider.value = currentHealth / maxHealth;
        }    
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }
}
