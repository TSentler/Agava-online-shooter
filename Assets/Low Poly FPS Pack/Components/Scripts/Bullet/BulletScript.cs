using UnityEngine;
using System.Collections;
using Photon.Pun;

public class BulletScript : MonoBehaviour {

	[Range(5, 100)]
	[Tooltip("After how long time should the bullet prefab be destroyed?")]
	public float destroyAfter;
	[Tooltip("If enabled the bullet destroys on impact")]
	public bool destroyOnImpact = false;
	[Tooltip("Minimum time after impact that the bullet is destroyed")]
	public float minDestroyTime;
	[Tooltip("Maximum time after impact that the bullet is destroyed")]
	public float maxDestroyTime;

	[Header("Impact Effect Prefabs")]
	public Transform [] bloodImpactPrefabs;
	public Transform [] metalImpactPrefabs;
	public Transform [] dirtImpactPrefabs;
	public Transform []	concreteImpactPrefabs;

	private float _damage;
	private IShooting _gun;
	private Camera _camera;

	private void Start () 
	{
		//Start destroy timer
		StartCoroutine (DestroyAfter ());

		RaycastHit hit;
		var startPosition = transform.position + transform.forward * 0.5f;

		if (Physics.Raycast(startPosition, transform.forward, out hit))
        {

			Debug.DrawRay(startPosition, transform.forward * 0.5f, Color.cyan, 100f);
			if (hit.collider.TryGetComponent(out HitDetector hitDetector))
			{
				if (hitDetector.PhotonView.IsMine == false)
				{
					hitDetector.DetectHit(_damage, PhotonNetwork.LocalPlayer);
					_gun.HitOnPlayer();
				}
			}

			var other = hit;
			//If bullet collides with "Blood" tag
			if (other.transform.tag == "Blood") 
			{			
				//Instantiate random impact prefab from array
				Instantiate (bloodImpactPrefabs [Random.Range 
					(0, bloodImpactPrefabs.Length)], other.point, 
					Quaternion.LookRotation(other.normal));
				//Destroy bullet object
				Destroy(gameObject);
			}

			//If bullet collides with "Metal" tag
			if (other.transform.tag == "Metal") 
			{
				//Instantiate random impact prefab from array
				Instantiate (metalImpactPrefabs [Random.Range 
					(0, bloodImpactPrefabs.Length)], other.point, 
					Quaternion.LookRotation (other.normal));
				//Destroy bullet object
				Destroy(gameObject);
			}

			//If bullet collides with "Dirt" tag
			if (other.transform.tag == "Dirt") 
			{
				//Instantiate random impact prefab from array
				Instantiate (dirtImpactPrefabs [Random.Range 
					(0, bloodImpactPrefabs.Length)], other.point, 
					Quaternion.LookRotation (other.normal));
				//Destroy bullet object
				Destroy(gameObject);
			}			
		}
	}

    //If the bullet collides with anything
    private void OnTriggerEnter(Collider other) 
	{
		var collisionPoint = other.ClosestPoint(transform.position);
		var collisionNormal = transform.position - collisionPoint;

		//If destroy on impact is false, start 
		//coroutine with random destroy timer
		if (!destroyOnImpact) 
		{
			StartCoroutine (DestroyTimer());
		}
		//Otherwise, destroy bullet on impact
		else 
		{
			Destroy (gameObject);
		}
	}

	public void SetDamage(float damage)
    {
		_damage = damage;
    }

	public void SetGun(IShooting gun)
    {
		_gun = gun;
	}

	private IEnumerator DestroyTimer () 
	{
		//Wait random time based on min and max values
		yield return new WaitForSeconds
			(Random.Range(minDestroyTime, maxDestroyTime));
		//Destroy bullet object
		Destroy(gameObject);
	}

	private IEnumerator DestroyAfter () 
	{
		//Wait for set amount of time
		yield return new WaitForSeconds (destroyAfter);
		//Destroy bullet object
		Destroy (gameObject);
	}
}