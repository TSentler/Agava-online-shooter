using PlayerAbilities;
using System;
using System.Collections;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] private protected float _delayPerShoot;
    [SerializeField] private protected float _delayReload;
    [SerializeField] private protected int _maxAmmo;
    [SerializeField] private protected int _id;
    [SerializeField] protected MouseLook MouseLook;

    private protected int _ammoQuanity;
    private protected bool _canShoot = true;

    public int MaxAmmo => _maxAmmo;
    public int AmmoQuanity => _ammoQuanity;
    public int GunID => _id;

    public event Action Hit;

    public abstract void Shoot(Camera camera);

    private void Start()
    {
        _ammoQuanity = _maxAmmo;
    }

    public void Reload()
    {
        if (_ammoQuanity < MaxAmmo)
            StartCoroutine(RestoreAmmo());
    }

    private protected void OnHit()
    {
        Hit?.Invoke();
    }

    private protected IEnumerator CountdownShoot()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_delayPerShoot);
        _canShoot = true;
    }

    private IEnumerator RestoreAmmo()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_delayReload);
        _canShoot = true;
        _ammoQuanity = _maxAmmo;
    }
}
