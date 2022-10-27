using Photon.Pun;
using PlayerAbilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] private float _spread;
    [SerializeField] private int _coutOfShardsInOneBullet;

    private float _fireaInterval;

    private void Update()
    {
        if (PhotonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot(Camera);
                CanShoot = false;

                if (MaxAmmoQuanity <= 0)
                {
                    AmmoQuanity = MaxAmmo;
                    MaxAmmoQuanity = MaxAmmoCount;
                    CanShoot = true;
                    OnEmptyAmmo();
                }
            }
        }

        _fireaInterval += Time.deltaTime;

        if (_fireaInterval >= DelayPerShoot)
        {
            CanShoot = true;
            _fireaInterval = 0;
        }
    }

    protected override void Shoot(Camera camera)
    {
        if (AmmoQuanity > 0 && CanShoot)
        {
            ShootParticle.Play();      
            ShootSound.Play();

            for (var y = 0; y < _coutOfShardsInOneBullet; y++)
            {
                Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                Vector3 spreadVector = /*camera.transform.position +*/ (Random.insideUnitSphere * _spread);

                ray.origin = (camera.transform.position + spreadVector);
                Debug.DrawRay(camera.transform.position, spreadVector,Color.green,100,true);
                RaycastHit[] hits = Physics.RaycastAll(ray, LayerToDetect);

                RaycastHit minDistanceHit = new RaycastHit
                {
                    distance = MinDistanceHit
                };

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.gameObject.TryGetComponent(out HitDetector hitDetector))
                    {
                        if (hitDetector.PhotonView.IsMine == false)
                        {
                            hitDetector.DetectHit(Damage, PhotonNetwork.LocalPlayer);
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
            }

            MouseLook.Shoot(RecoilForceXMin, RecoilForceYMin, RecoilForceXMax, RecoilForceYMax);
            AmmoQuanity--;

            if (MaxAmmoQuanity != 0)
            {
                MaxAmmoQuanity--;
            }
            else
            {
                if (AmmoQuanity == 0)
                    Reload();
            }        
        }

    }
}

