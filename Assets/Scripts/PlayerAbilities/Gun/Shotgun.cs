using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
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
}
