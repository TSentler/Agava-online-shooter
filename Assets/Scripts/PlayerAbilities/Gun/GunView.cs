using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunView : MonoBehaviour
{
    [SerializeField] private WeaponsHolder _weaponsHolder;
    [SerializeField] private Canvas _playerCanvas;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Image _hitIndicator;
    [SerializeField] private DamageIndicatorText _damageText;

  
    private float _timeToShowIndicator = 0.1f;

    private void Start()
    {
        if (!_photonView.IsMine)
        {
            Destroy(_playerCanvas.gameObject);            
        }      
    }

    private void OnEnable()
    {
        //_weaponsHolder.UpdateAmmo += OnGunChanged;

        //for (int i = 0; i < _weaponsHolder.Guns.Count; i++)
        //{
        //    _weaponsHolder.Guns[i].Hit += OnHit;
        //}
    }

    private void OnDisable()
    {
        //_weaponsHolder.UpdateAmmo -= OnGunChanged;

        //for (int i = 0; i < _weaponsHolder.Guns.Count; i++)
        //{
        //    _weaponsHolder.Guns[i].Hit -= OnHit;
        //}
    }

    //private void OnGunChanged(int ammoQuanity, int maxAmmo)
    //{
    //    _currentAmmoText.text = ammoQuanity.ToString();
    //    _totalAmmoText.text = maxAmmo.ToString();
    //}


    public void OnHit(float damage)
    {
        StartCoroutine(ShowHitIndicator());
        DamageIndicatorText damageText = Instantiate(_damageText, _hitIndicator.transform.position, Quaternion.identity);
        damageText.gameObject.transform.SetParent(_playerCanvas.transform);
        damageText.SetDamageText(damage);
    }

    private IEnumerator ShowHitIndicator()
    {
        _hitIndicator.gameObject.SetActive(true);

        yield return new WaitForSeconds(_timeToShowIndicator);

        _hitIndicator.gameObject.SetActive(false);
    }
}
