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

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                PhotonView.RPC(nameof(ShootRpc), RpcTarget.All, hit.point, hit.normal);

                if (hit.collider.gameObject.TryGetComponent(out PlayerHealth playerHealth))
                {
                    if(playerHealth.PhotonView.IsMine == false)
                    {
                        playerHealth.ApplyDamage(_damage, PhotonNetwork.LocalPlayer);
                        OnHit();
                    }                 
                }
            }

            StartCoroutine(CountdownShoot());
            _ammoQuanity--;
        }
        else
        {
            if (_ammoQuanity == 0 && _canShoot)
            {
                Reload();
                Debug.Log("Reloading");
            }
        }
    }
}
