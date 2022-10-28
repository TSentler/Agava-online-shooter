using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : Gun
{
    private float _fireInterval;

    private void Update()
    {
        if (PhotonView.IsMine && PlayerPhotonView.IsBot == false)
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

        _fireInterval += Time.deltaTime;

        if (_fireInterval >= DelayPerShoot)
        {
            CanShoot = true;
            _fireInterval = 0;
        }
    }
}
