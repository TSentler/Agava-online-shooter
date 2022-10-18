using Photon.Pun;
using PlayerAbilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    [SerializeField] private float _damage;
    [SerializeField] private ParticleSystem _shootParticle;
    [SerializeField] private float _recoilForce;

    public override void Shoot(Camera camera)
    {
        _shootParticle.Play();
        MouseLook.Shoot(_recoilForce);

        if (_ammoQuanity > 0 && _canShoot)
        {
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = camera.transform.position;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out PlayerHealth playerHealth))
                {
                    playerHealth.ApplyDamage(_damage, PhotonNetwork.LocalPlayer);
                    OnHit();
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
