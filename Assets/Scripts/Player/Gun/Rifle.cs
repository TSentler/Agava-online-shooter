using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Gun
{
    [SerializeField] private float _damage;

    public override void Shoot(Camera camera)
    {
        if (AmmoQuanity > 0 && CanShoot)
        {
            Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = camera.transform.position;

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.TryGetComponent(out PlayerHealth playerHealth))
                {
                    playerHealth.ApplyDamage(_damage, PhotonNetwork.LocalPlayer);
                }
            }
            StartCoroutine(CountdownShoot());
            AmmoQuanity--;
            Debug.Log(AmmoQuanity);
        }
        else
        {
            if (AmmoQuanity == 0)
            {
                Reload();
                Debug.Log("Reloading");
            }
        }
    }
}
