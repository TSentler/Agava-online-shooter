using Photon.Pun;
using UnityEngine;

public class Pistol : Gun
{
    private void Update()
    {
        if (PhotonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
                Shoot(Camera);
        }
    }
}
