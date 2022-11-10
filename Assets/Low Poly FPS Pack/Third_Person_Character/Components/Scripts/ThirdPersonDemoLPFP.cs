using System.Collections;
using System.Collections.Generic;
using CharacterInput;
using Photon.Pun;
using UnityEngine;

public class ThirdPersonDemoLPFP : MonoBehaviour {
	[Header("Camera")]
	public Camera mainCamera;
	[Header("Camera FOV Settings")]
	public float zoomedFOV;
	public float defaultFOV;
	[Tooltip("How fast the camera zooms in")]
	public float fovSpeed;

	private Animator anim;

	[Header("Weapon Settings")]
	public bool semi;
	public bool auto;

	//Used for fire rate
	private float lastFired;
	//How fast the weapon fires, higher value means faster rate of fire
	[Tooltip("How fast the weapon fires, higher value means faster rate of fire.")]
	public float fireRate;

	[Header("Weapon Components")]
	public ParticleSystem muzzleflashParticles;
	public Light muzzleflashLight;

	[Header("Prefabs")]
	public Transform casingPrefab;
	public Transform bulletPrefab;
	public float bulletForce;
	public Transform grenadePrefab;
	public float grenadeSpawnDelay;

	[Header("Spawnpoints")]
	public Transform casingSpawnpoint;
	public Transform bulletSpawnpoint;
	public Transform grenadeSpawnpoint;

	[Header("Audio Clips")]
	public AudioClip shootSound;

	[Header("Audio Sources")]
	public AudioSource shootAudioSource;
	
    [SerializeField] private MonoBehaviour _inputSourceBehaviour;
    [SerializeField] private PhotonView _photonView;
	
	private ICharacterInputSource _inputSource;
	
	private void OnValidate()
	{
		if (_inputSourceBehaviour 
			&& !(_inputSourceBehaviour is ICharacterInputSource))
		{
			Debug.LogError(nameof(_inputSourceBehaviour) + " needs to implement " + nameof(ICharacterInputSource));
			_inputSourceBehaviour = null;
		}
	}

	public void Initialize(ICharacterInputSource inputSource)
	{
		_inputSource = inputSource;
	}
	
	private void Awake()
	{
		if (_inputSource == null)
		{
			Initialize((ICharacterInputSource)_inputSourceBehaviour);
		}
	}
	
	private void Start () 
	{
		//Assign animator component
		anim = gameObject.GetComponent<Animator> ();
		//Disable muzzleflash light at start
		muzzleflashLight.enabled = false;
	}

	private void Update () 
	{
		if (_photonView.IsMine == false)
			return;
		
		//Aim in with right click hold
		if (Input.GetMouseButton (1)) 
		{
			//Increase camera field of view
			mainCamera.fieldOfView = Mathf.Lerp (mainCamera.fieldOfView,
				zoomedFOV, fovSpeed * Time.deltaTime);
		} 
		else 
		{
			//Restore camera field of view
			mainCamera.fieldOfView = Mathf.Lerp (mainCamera.fieldOfView,
				defaultFOV, fovSpeed * Time.deltaTime);
		}

		var moveInput = _inputSource.MovementInput;
		anim.SetFloat ("Vertical", moveInput.y, 0, Time.deltaTime);
		anim.SetFloat ("Horizontal", moveInput.x, 0, Time.deltaTime);

		//Single fire with left click
		if (Input.GetMouseButtonDown (0) && semi == true) 
		{
			//Play shoot sound 
			//shootAudioSource.clip = shootSound;
			//shootAudioSource.Play ();

			//Play from second layer, from the beginning
			anim.Play ("Fire", 1, 0.0f);
			//Play muzzleflash particles
			muzzleflashParticles.Emit (1);
			//Play light flash
			StartCoroutine (MuzzleflashLight ());

			//Spawn casing at spawnpoint
			//Instantiate (casingPrefab, 
			//	casingSpawnpoint.transform.position, 
			//	casingSpawnpoint.transform.rotation);
		}

		//AUtomatic fire
		//Left click hold 
		if (Input.GetMouseButton (0) && auto == true) 
		{
			//Shoot automatic
			if (Time.time - lastFired > 1 / fireRate) 
			{
				lastFired = Time.time;
				//Play shoot sound
				//shootAudioSource.clip = shootSound;
				//shootAudioSource.Play ();

				//Play from second layer, from the beginning
				anim.Play ("Fire", 1, 0.0f);
				//Play muzzleflash particles
				muzzleflashParticles.Emit (1);
				//Play light flash
				StartCoroutine (MuzzleflashLight ());

				//Spawn casing at spawnpoint
				Instantiate (casingPrefab, 
					casingSpawnpoint.transform.position, 
					casingSpawnpoint.transform.rotation);

				//Spawn bullet from bullet spawnpoint
				//var bullet = (Transform)Instantiate (
				//	bulletPrefab,
				//	bulletSpawnpoint.transform.position,
				//	bulletSpawnpoint.transform.rotation);

				//Add velocity to the bullet
				//bullet.GetComponent<Rigidbody> ().velocity = 
				//	bullet.transform.forward * bulletForce;
			}
		}

		//Reload with R key for testing
		if (Input.GetKeyDown (KeyCode.R)) {
			//Play reload animation
			anim.Play("Reload", 1, 0.0f);
		}

		//Throw grenade when pressing G key
		if (Input.GetKeyDown (KeyCode.G)) 
		{
			//StartCoroutine (GrenadeSpawnDelay ());
			//Play grenade throw animation
			anim.Play("Grenade_Throw", 1, 0.0f);
		}
	}

	private IEnumerator GrenadeSpawnDelay () 
	{
		//Wait for set amount of time before spawning grenade
		yield return new WaitForSeconds (grenadeSpawnDelay);
		//Spawn grenade prefab at spawnpoint
		Instantiate(grenadePrefab, 
			grenadeSpawnpoint.transform.position, 
			grenadeSpawnpoint.transform.rotation);
	}

	IEnumerator MuzzleflashLight () 
	{
		muzzleflashLight.enabled = true;
		yield return new WaitForSeconds (0.02f);
		muzzleflashLight.enabled = false;
	}
	
}