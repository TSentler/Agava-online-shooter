using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunView : MonoBehaviour
{
    [SerializeField] private PlayerHand _playerHand;
    [SerializeField] private TMP_Text _ammoQuanityText;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Image _crosshair;
    [SerializeField] private Image _hitIndicator;

    private float _timeToShowIndicator = 0.1f;

    private void Start()
    {
        if (!_photonView.IsMine)
        {
            Destroy(_ammoQuanityText.gameObject);
            Destroy(_crosshair.gameObject);
            Destroy(_hitIndicator.gameObject);
        }
    }

    private void OnEnable()
    {
        _playerHand.GunChanged += OnGunChanged;

        for (int i = 0; i < _playerHand.Guns.Count; i++)
        {
            _playerHand.Guns[i].Hit += OnHit;
        }
    }

    private void OnDisable()
    {
        _playerHand.GunChanged -= OnGunChanged;

        for (int i = 0; i < _playerHand.Guns.Count; i++)
        {
            _playerHand.Guns[i].Hit -= OnHit;
        }
    }

    private void OnGunChanged(int ammoQuanity, int maxAmmo)
    {
        _ammoQuanityText.text = $"{ammoQuanity}/{maxAmmo}";
    }

    private void OnHit()
    {
        StartCoroutine(ShowHitIndicator());
    }

    private IEnumerator ShowHitIndicator()
    {
        _hitIndicator.gameObject.SetActive(true);

        yield return new WaitForSeconds(_timeToShowIndicator);

        _hitIndicator.gameObject.SetActive(false);
    }
}
