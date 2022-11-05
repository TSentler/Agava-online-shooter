using Photon.Pun;
using UnityEngine;

public class Rifle : Gun
{
    private float _fireaInterval;
    private float _nextShootTime;

    private void Update()
    {
        if (CanShoot == true)
            return;

        _fireaInterval += Time.deltaTime;

        if (_fireaInterval >= _nextShootTime)
        {
            CanShoot = true;
            //_fireaInterval = 0;
        }
    }

    private void FixedUpdate()
    {
        if (PhotonViewComponent != null)
        {
            if (PhotonViewComponent.IsMine && _playerInfo.IsBot == false)
            {
                if(CanShoot == true)
                {
                    if (Input.GetMouseButton(0))
                    {
                        Shoot(Camera);
                        CanShoot = false;
                        _nextShootTime = Time.time + DelayPerShoot;
                        _fireaInterval = Time.time;

                        if (MaxAmmoQuanity <= 0)
                        {
                            AmmoQuanity = MaxAmmo;
                            MaxAmmoQuanity = MaxAmmoCount;
                            CanShoot = true;
                            OnEmptyAmmo();
                        }
                    }
                }
                
                if (NeedReload)
                {
                    Reload();
                }
            }
        }
    }
}
