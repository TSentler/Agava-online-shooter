using Photon.Pun;
using PlayerAbilities;
using UnityEngine;

public class Pistol : Gun
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
            }
        }
        if (NeedReload)
        {
            Reload();
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
