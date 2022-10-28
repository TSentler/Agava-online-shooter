using Photon.Pun;
using PlayerAbilities;
using UnityEngine;

public class Pistol : Gun
{
    private void Update()
    {
        if (PhotonView.IsMine && PlayerPhotonView.IsBot == false)
        {
            if (Input.GetMouseButtonDown(0))
                Shoot(Camera);
        }
    }
}
