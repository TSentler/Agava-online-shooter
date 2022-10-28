using Photon.Pun;
using PlayerAbilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    [SerializeField] private float _spread;
    [SerializeField] private int _coutOfShardsInOneBullet;
    [SerializeField] private float _heightMultiplier;

    private float _fireaInterval;

    private void Update()
    {
        if (PhotonView.IsMine && _playerInfo.IsBot == false)
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

    protected override void Shoot(Camera camera)
    {
        if (AmmoQuanity > 0 && CanShoot)
        {
            ShootParticle.Play();      
            ShootSound.Play();

            for (var y = 0; y < _coutOfShardsInOneBullet; y++)
            {
                Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                Vector3 spreadVector = camera.transform.position + (Random.insideUnitSphere * _spread);
                ray.origin = (camera.transform.position + spreadVector);


                //float angle = y * (_spread / (_coutOfShardsInOneBullet - 1));
                //Vector3 direction = Quaternion.AngleAxis(angle + (180 - (_spread / 2)), transform.up) * new Vector3(0, 0, -360);
                //Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                //Debug.DrawRay(camera.transform.position, spreadVector,Color.green,100,true);
                RaycastHit[] hits = Physics.RaycastAll(ray, LayerToDetect);
                //Physics physics = new Physics() ;
                //RaycastHit[] hits = physics.ConeCastAll(transform.position, 3, transform.forward, 2, 3);
                //RaycastHit[] hits = Physics.C

                RaycastHit minDistanceHit = new RaycastHit
                {
                    distance = MinDistanceHit
                };

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.gameObject.TryGetComponent(out HitDetector hitDetector))
                    {
                        if (hitDetector.IsMine == false || hitDetector.IsBot)
                        {
                            hitDetector.DetectHit(Damage, PhotonNetwork.LocalPlayer);
                            OnHit();
                        }
                        break;
                    }
                    else
                    {
                        for (int j = 0; j < hits.Length; j++)
                        {
                            if (hits[j].distance < minDistanceHit.distance)
                                minDistanceHit = hits[j];
                        }

                        if (minDistanceHit.collider.gameObject.TryGetComponent(out PlayerHealth playerHealth) == false &&
                            minDistanceHit.collider.gameObject.TryGetComponent(out HitDetector hit) == false)
                            PhotonView.RPC(nameof(ShootRpc), RpcTarget.All, minDistanceHit.point, minDistanceHit.normal);
                    }
                }
            }

            MouseLook.Shoot(RecoilForceXMin, RecoilForceYMin, RecoilForceXMax, RecoilForceYMax);
            AmmoQuanity--;

            if (MaxAmmoQuanity != 0)
            {
                MaxAmmoQuanity--;
            }
            else
            {
                if (AmmoQuanity == 0)
                    Reload();
            }        
        }

    }
}


//public static class ConeCastExtension
//{
//    public static RaycastHit[] ConeCastAll(this Physics physics, Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle)
//    {
//        RaycastHit[] sphereCastHits = Physics.SphereCastAll(origin - new Vector3(0, 0, maxRadius), maxRadius, direction, maxDistance);
//        List<RaycastHit> coneCastHitList = new List<RaycastHit>();

//        //if (sphereCastHits.Length > 0)
//        //{
//        //    for (int i = 0; i < sphereCastHits.Length; i++)
//        //    {
//        //        sphereCastHits[i].collider.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
//        //        Vector3 hitPoint = sphereCastHits[i].point;
//        //        Vector3 directionToHit = hitPoint - origin;
//        //        float angleToHit = Vector3.Angle(direction, directionToHit);

//        //        if (angleToHit < coneAngle)
//        //        {
//        //            coneCastHitList.Add(sphereCastHits[i]);
//        //        }
//        //    }
//        //}

//        RaycastHit[] coneCastHits = new RaycastHit[coneCastHitList.Count];
//        coneCastHits = coneCastHitList.ToArray();

//        return coneCastHits;
//    }
//}
