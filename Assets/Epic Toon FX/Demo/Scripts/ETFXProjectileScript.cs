using UnityEngine;
using System.Collections;
using Photon.Pun;
using PlayerAbilities;

public class ETFXProjectileScript : MonoBehaviour
{
    [SerializeField] private GameObject _impactParticle; // Effect spawned when projectile hits a collider
    [SerializeField] private GameObject _projectileParticle; // Effect attached to the gameobject as child
    [SerializeField] private GameObject _muzzleParticle; // Effect instantly spawned when gameobject is spawned
    [Header("Adjust if not using Sphere Collider")]
    [SerializeField] private float colliderRadius = 1f;
    [Range(0f, 1f)] // This is an offset that moves the impact effect slightly away from the point of impact to reduce clipping of the impact effect
    [SerializeField] private float collideOffset = 0.15f;
    [SerializeField] private GameObject _bulletHoleTemplate;
    [SerializeField] private GameObject _hitInPlayerEffect;

    private PhotonView _photonView;

    private void Start()
    {
        _projectileParticle = PhotonNetwork.Instantiate(_projectileParticle.name, transform.position, transform.rotation) as GameObject;
        _projectileParticle.transform.parent = transform;

        if (_muzzleParticle)
        {
            _muzzleParticle = PhotonNetwork.Instantiate(_muzzleParticle.name, transform.position, transform.rotation) as GameObject;
           PhotonNetwork.Destroy(_muzzleParticle); // 2nd parameter is lifetime of effect in seconds
        }

        StartCoroutine(DestroyWithDelay());
    }

    private void FixedUpdate()
    {
        if (GetComponent<Rigidbody>().velocity.magnitude != 0)
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity); // Sets rotation to look at direction of movement
        }

        RaycastHit hit;

        float radius; // Sets the radius of the collision detection
        if (transform.GetComponent<SphereCollider>())
            radius = transform.GetComponent<SphereCollider>().radius;
        else
            radius = colliderRadius;

        Vector3 direction = transform.GetComponent<Rigidbody>().velocity; // Gets the direction of the projectile, used for collision detection
        if (transform.GetComponent<Rigidbody>().useGravity)
            direction += Physics.gravity * Time.deltaTime; // Accounts for gravity if enabled
        direction = direction.normalized;

        float detectionDistance = transform.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime; // Distance of collision detection for this frame

        if (Physics.SphereCast(transform.position, radius, direction, out hit, detectionDistance)) // Checks if collision will happen
        {
            if (hit.collider.gameObject.GetComponent<Grenade>())
                return;

            transform.position = hit.point + (hit.normal * collideOffset); // Move projectile to point of collision
            GameObject impactParticle = null;

            if (hit.collider.gameObject.GetComponent<PlayerHealth>() || hit.collider.gameObject.GetComponent<HitDetector>())
            {
                Instantiate(_hitInPlayerEffect, transform.position, Quaternion.LookRotation(hit.normal));
                Debug.Log(1);
            }
            else
            {
                impactParticle = PhotonNetwork.Instantiate(_impactParticle.name, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject; // Spawns impact effect
                PhotonNetwork.Instantiate(_bulletHoleTemplate.name, transform.position, Quaternion.LookRotation(hit.normal));
                Debug.Log(2);
            }
                 
            ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>(); // Gets a list of particle systems, as we need to detach the trails
                                                                                 //Component at [0] is that of the parent i.e. this object (if there is any)
            for (int i = 1; i < trails.Length; i++) // Loop to cycle through found particle systems
            {
                ParticleSystem trail = trails[i];

                if (trail.gameObject.name.Contains("Trail"))
                {
                    trail.transform.SetParent(null); // Detaches the trail from the projectile
                    Destroy(trail.gameObject, 2f); // Removes the trail after seconds
                }
            }

            Destroy(_projectileParticle, 3f); // Removes particle effect after delay

            if(impactParticle != null)
            {
                Destroy(impactParticle, 3.5f); // Removes impact effect after delay
            }
           
            PhotonNetwork.Destroy(gameObject); // Removes the projectile
        }
    }

    private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(30f);
        PhotonNetwork.Destroy(gameObject);
    }
}