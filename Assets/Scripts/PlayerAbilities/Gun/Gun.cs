using Photon.Pun;
using PlayerAbilities;
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Gun : MonoBehaviour
{
    private const float _radiusSphereCollider = 0.3f;
    private const float _timeToDestroyBullet = 2f;
    private const float _stepToSpawnPosition = 0.001f;
    private const float _bulletTravelTime = 0.05f;

    [SerializeField] protected float DelayPerShoot;
    [SerializeField] protected float DelayReload;
    [SerializeField] protected int MaxAmmo;
    [SerializeField] protected int Id;
    [SerializeField] protected PhotonView PhotonViewComponent;
    [SerializeField] protected PlayerInfo _playerInfo;
    [SerializeField] protected Camera Camera;
    [SerializeField] protected int MaxAmmoCount;
    [SerializeField] protected ParticleSystem ShootParticle;
    [SerializeField] protected AudioSource ShootSound;
    [SerializeField] protected MouseLook MouseLook;
    [SerializeField] protected float Damage;
    [SerializeField] protected float RecoilForceXMax;
    [SerializeField] protected float RecoilForceYMax;
    [SerializeField] protected float RecoilForceXMin;
    [SerializeField] protected float RecoilForceYMin;
    [SerializeField] protected LayerMask LayerToDetect;
    [SerializeField] protected float RecoilMagnitude;

    [SerializeField] private ParticleSystem _bulletHoleTemplate;
    [SerializeField] private GameObject _bulletParticle;
    [SerializeField] private float _bulletForce;
    [SerializeField] private Transform _bulletSpawnPosition;

    private protected int AmmoQuanity;
    private protected int MaxAmmoQuanity;
    private protected float MinDistanceHit = 1000f;
    private protected bool CanShoot = true;
    private protected bool NeedReload = false;

    private float _remainingReloadTime;

    public int MaxAmmoGun => MaxAmmo;
    public int AmmoQuanityGun => AmmoQuanity;
    public int GunID => Id;

    public event Action Hit;
    public event UnityAction EmptyAmmo;
    public event UnityAction<int, int> AmmoCountChange;

    private void Start()
    {
        AmmoQuanity = MaxAmmo;
        MaxAmmoQuanity = MaxAmmoCount;
        _remainingReloadTime = DelayReload;
        AmmoCountChange?.Invoke(AmmoQuanity, MaxAmmoCount == 0 ? MaxAmmo : MaxAmmoQuanity);
    }

    public void Reload()
    {
        if (AmmoQuanity < MaxAmmo)
        {
            NeedReload = true;
            CanShoot = false;
            _remainingReloadTime -= Time.deltaTime;

            if (_remainingReloadTime <= 0)
            {
                CanShoot = true;
                NeedReload = false;

                if (MaxAmmoQuanity <= MaxAmmo && MaxAmmoCount != 0)
                {
                    AmmoQuanity = MaxAmmoQuanity;
                }
                else
                {
                    AmmoQuanity = MaxAmmo;
                }

                _remainingReloadTime = DelayReload;
                AmmoCountChange?.Invoke(AmmoQuanity, MaxAmmoCount == 0 ? MaxAmmo : MaxAmmoQuanity);
            }
        }
    }

    protected void Shoot(Camera camera)
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = camera.transform.position;
        Shoot(ray, camera.transform);
    }
    
    public virtual void Shoot(Ray ray, Transform originTransform)
    {
        if (AmmoQuanity > 0 && CanShoot)
        {
            PhotonViewComponent.RPC(nameof(PlayEffects), RpcTarget.All);
            ShootSound.Play();


            RaycastHit[] hits = Physics.RaycastAll(ray, LayerToDetect);

            RaycastHit minDistanceHit = new RaycastHit
            {
                distance = MinDistanceHit
            };

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.TryGetComponent(out HitDetector hitDetector))
                {

                    if (hitDetector.IsMine && hitDetector.IsSameRootTransform(
                            _playerInfo.transform))
                    {
                        continue;
                    }
                    
                    hitDetector.DetectHit(Damage, PhotonNetwork.LocalPlayer);
                    OnHit();
                    break;
                }
                else
                {
                    for (int j = 0; j < hits.Length; j++)
                    {
                        if (hits[j].distance < minDistanceHit.distance)
                            minDistanceHit = hits[j];
                    }

                    //if (minDistanceHit.collider.gameObject.TryGetComponent(out PlayerHealth playerHealth) == false &&
                    //    minDistanceHit.collider.gameObject.TryGetComponent(out HitDetector hit) == false)
                    //    PhotonView.RPC(nameof(ShootRpc), RpcTarget.All, minDistanceHit.point, minDistanceHit.normal);
                }
            }

            _bulletForce = (minDistanceHit.distance / _bulletTravelTime) / _bulletTravelTime;

            GameObject bulletParticle = PhotonNetwork.Instantiate(_bulletParticle.name, _bulletSpawnPosition.position, Quaternion.identity);
            bulletParticle.GetComponent<Rigidbody>().AddForce(originTransform.forward * _bulletForce);
            bulletParticle.transform.LookAt(ray.origin);
            AmmoQuanity--;

            if (MaxAmmoQuanity != 0)
                MaxAmmoQuanity--;

            MouseLook.Shoot(RecoilForceXMin, RecoilForceYMin, RecoilForceXMax, RecoilForceYMax, RecoilMagnitude, DelayPerShoot);
            AmmoCountChange?.Invoke(AmmoQuanity, MaxAmmoCount == 0 ? MaxAmmo : MaxAmmoQuanity);
        }
        else
        {
            if (AmmoQuanity == 0)
            {
                Reload();
            }
        }
    }

    [PunRPC]
    protected void PlayEffects()
    {
        ShootParticle.Play();
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

        Quaternion rotation = Quaternion.LookRotation(hitNormal);

        if (colliders.Length != 0)
        {
            Instantiate(_bulletHoleTemplate, hitPosition + hitNormal * _stepToSpawnPosition, rotation);
        }
    }

    //private IEnumerator RestoreAmmo()
    //{
    //    CanShoot = false;
    //    yield return new WaitForSeconds(DelayReload);
    //    CanShoot = true;
    //    AmmoQuanity = MaxAmmo;
    //}
}
