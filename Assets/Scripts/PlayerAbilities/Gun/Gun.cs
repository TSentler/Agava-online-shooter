using Photon.Pun;
using PlayerAbilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] private protected float _delayPerShoot;
    [SerializeField] private protected float _delayReload;
    [SerializeField] private protected int _maxAmmo;
    [SerializeField] protected int Id;
    [SerializeField] protected GameObject BulletHoleTemplate;
    [SerializeField] protected MouseLook MouseLook;
    [SerializeField] protected AudioSource ShootSound;
    [SerializeField] protected PhotonView PhotonView;
    [SerializeField] protected Camera Camera;

    private protected int _ammoQuanity;
    private protected bool _canShoot = true;

    private const float _radiusSphereCollider = 0.3f;
    private const float _timeToDestroyBullet = 2f;
    private const float _stepToSpawnPosition = 0.001f;

    public int MaxAmmo => _maxAmmo;
    public int AmmoQuanity => _ammoQuanity;
    public int GunID => Id;

    public event Action Hit;
    public event UnityAction EmptyAmmo;

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

    private protected void OnEmptyAmmo()
    {
        EmptyAmmo?.Invoke();
    }

    //private protected IEnumerator CountdownShoot()
    //{
    //    _canShoot = false;
    //    yield return new WaitForSeconds(_delayPerShoot);
    //    _canShoot = true;
    //}

    private IEnumerator RestoreAmmo()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_delayReload);
        _canShoot = true;
        _ammoQuanity = _maxAmmo;
    }

    [PunRPC]
    private protected void ShootRpc(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, _radiusSphereCollider);

        Quaternion rotation = Quaternion.LookRotation(hitNormal, Vector3.up) * BulletHoleTemplate.transform.rotation;

        if (colliders.Length != 0)
        {
            var bulletHole = Instantiate(BulletHoleTemplate, hitPosition + hitNormal * _stepToSpawnPosition, rotation);
            bulletHole.transform.SetParent(colliders[0].transform);
            Destroy(bulletHole, _timeToDestroyBullet);
        }
    }
}
