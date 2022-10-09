using UnityEngine;
using Photon.Pun;

public class BulletSpaner : MonoBehaviour
{
    [SerializeField] private Bullet _bulletTemplate;
    [SerializeField] private PhotonView _photonView;

    private void Update()
    {
        if (_photonView.IsMine)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                GameObject bullet = PhotonNetwork.Instantiate("Bullet", transform.position, Quaternion.identity,0);
                
                bullet.GetComponent<Bullet>().Move();
            }
        }
    }
}
