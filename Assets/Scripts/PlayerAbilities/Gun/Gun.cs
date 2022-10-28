using Photon.Pun;
using PlayerAbilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Gun : MonoBehaviour
{
    private const float _radiusSphereCollider = 0.3f;
    private const float _timeToDestroyBullet = 2f;
    private const float _stepToSpawnPosition = 0.001f;

    [SerializeField] protected float DelayPerShoot;
    [SerializeField] protected float DelayReload;
    [SerializeField] protected int MaxAmmo;
    [SerializeField] protected int Id;
    [SerializeField] protected PhotonView PhotonView;
    [SerializeField] protected PlayerInfo _playerInfo;
    [SerializeField] protected Camera Camera;
    [SerializeField] protected int MaxAmmoCount;

    [SerializeField] private float _damage;
    [SerializeField] private float _recoilForceXMax;
    [SerializeField] private float _recoilForceYMax;
    [SerializeField] private float _recoilForceXMin;
    [SerializeField] private float _recoilForceYMin;
    [SerializeField] private GameObject _bulletHoleTemplate;
    [SerializeField] private MouseLook _mouseLook;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private LayerMask _layerToDetect;
    [SerializeField] private ParticleSystem _shootParticle;

    private protected int AmmoQuanity;
    private protected bool CanShoot = true;
    private protected float MinDistanceHit = 1000f;
    private protected int MaxAmmoQuanity;
    
    public int MaxAmmoGun => MaxAmmo;
    public int AmmoQuanityGun => AmmoQuanity;
    public int GunID => Id;

    public event Action Hit;
    public event UnityAction EmptyAmmo;

    private void Start()
    {
        AmmoQuanity = MaxAmmo;
        MaxAmmoQuanity = MaxAmmoCount;
    }

    public void Reload()
    {
        if (AmmoQuanity < MaxAmmoGun)
            StartCoroutine(RestoreAmmo());
    }

    private protected void Shoot(Camera camera)
    {
        if (AmmoQuanity > 0 && CanShoot)
        {
            _shootParticle.Play();
            _mouseLook.Shoot(_recoilForceXMin, _recoilForceYMin, _recoilForceXMax, _recoilForceYMax);
            _shootSound.Play();

            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = camera.transform.position;

            RaycastHit[] hits = Physics.RaycastAll(ray, _layerToDetect);

            RaycastHit minDistanceHit = new RaycastHit
            {
                distance = MinDistanceHit
            };

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.TryGetComponent(out HitDetector hitDetector))
                {
                    if (hitDetector.IsMine == false || hitDetector.IsBot)
                    {
                        hitDetector.DetectHit(_damage, PhotonNetwork.LocalPlayer);
                        OnHit();
                    }
                    break;
                }
                else
                {
                    for (int j = 0; j < hits.Length; j++)
                    {
                        if (hits[j].distance < minDistanceHit.distance)
                            minDistanceHit = hits[j];
                    }

                    if (minDistanceHit.collider.gameObject.TryGetComponent(out PlayerHealth playerHealth) == false && 
                        minDistanceHit.collider.gameObject.TryGetComponent(out HitDetector hit) == false)
                        PhotonView.RPC(nameof(ShootRpc), RpcTarget.All, minDistanceHit.point, minDistanceHit.normal);
                }
            }

            AmmoQuanity--;

            if (MaxAmmoQuanity != 0)
                MaxAmmoQuanity--;

        }
        else
        {
            if (AmmoQuanity == 0)
                Reload();
        }
    }

    private protected void OnHit()
    {
        Hit?.Invoke();
    }

    private protected void OnEmptyAmmo()
    {
        EmptyAmmo?.Invoke();
    }

    [PunRPC]
    private protected void ShootRpc(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, _radiusSphereCollider);

        Quaternion rotation = Quaternion.LookRotation(hitNormal, Vector3.up) * _bulletHoleTemplate.transform.rotation;

        if (colliders.Length != 0)
        {
            var bulletHole = Instantiate(_bulletHoleTemplate, hitPosition + hitNormal * _stepToSpawnPosition, rotation);
            bulletHole.transform.SetParent(colliders[0].transform);
            Destroy(bulletHole, _timeToDestroyBullet);
        }
    }

    private IEnumerator RestoreAmmo()
    {
        CanShoot = false;
        yield return new WaitForSeconds(DelayReload);
        CanShoot = true;
        AmmoQuanity = MaxAmmo;
    }
}
