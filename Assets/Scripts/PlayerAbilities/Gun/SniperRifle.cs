using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperRifle : Gun
{
    private float _fireInterval;
    private float _nextShootTime;

    private void Update()
    {
        if (PhotonViewComponent.IsMine && _playerInfo.IsBot == false)
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

        if (CanShoot == true)
            return;

        _fireInterval += Time.deltaTime;

        if (_fireInterval >= _nextShootTime)
        {
            CanShoot = true;
            //_fireaInterval = 0;
        }
    }
}
