using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    [SerializeField] private float _damage;



    public override void Shoot(Camera camera)
    {
        if (_ammoQuanity > 0 && _canShoot)
        {
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = camera.transform.position;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out PlayerHealth playerHealth))
                {
                    playerHealth.ApplyDamage(_damage);
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
