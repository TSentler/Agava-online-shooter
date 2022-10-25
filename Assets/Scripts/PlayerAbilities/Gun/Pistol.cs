using Photon.Pun;
using PlayerAbilities;
using System;
using UnityEngine;

public class Pistol : Gun
{
    [SerializeField] private float _damage;
    [SerializeField] private ParticleSystem _shootParticle;
    [SerializeField] private float _recoilForceXMax;
    [SerializeField] private float _recoilForceYMax;
    [SerializeField] private float _recoilForceXMin;
    [SerializeField] private float _recoilForceYMin;

    private void Update()
    {
        if (PhotonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot(Camera);
            }
        }
    }

    public override void Shoot(Camera camera)
    {
        if (_ammoQuanity > 0 && _canShoot)
        {
            _shootParticle.Play();
            MouseLook.Shoot(_recoilForceXMin, _recoilForceYMin, _recoilForceXMax, _recoilForceYMax);
            ShootSound.Play();

            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = camera.transform.position;

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
                        {
                            minDistanceHit = hits[j];
                        }
                    }

                    if (minDistanceHit.collider.gameObject.TryGetComponent(out PlayerHealth playerHealth) == false)
                    {
                        PhotonView.RPC(nameof(ShootRpc), RpcTarget.All, minDistanceHit.point, minDistanceHit.normal);
                    }
                }
            }
            
            //StartCoroutine(CountdownShoot());
            _ammoQuanity--;
        }
        else
        {
            if (_ammoQuanity == 0)
            {
                Reload();
            }
        }
    }
}
