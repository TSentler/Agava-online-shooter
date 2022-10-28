using Photon.Pun;
using UnityEngine;

public class Rifle : Gun
{
    private float _fireaInterval;

    private void Update()
    {
        _fireaInterval += Time.deltaTime;

        if (_fireaInterval >= DelayPerShoot)
        {
            CanShoot = true;
            _fireaInterval = 0;
        }
    }

    private void FixedUpdate()
    {
        if (PhotonView != null)
        {
            if (PhotonView.IsMine)
            {
                if (Input.GetMouseButton(0))
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
        }
    }
}
