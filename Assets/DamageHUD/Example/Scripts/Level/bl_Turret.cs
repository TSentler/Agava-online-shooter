using UnityEngine;

public class bl_Turret : MonoBehaviour
{
   [SerializeField]private Transform Target = null;
   [SerializeField]private Transform Canom = null;
   [SerializeField]private Transform FirePosition = null;
   [SerializeField]private GameObject BallPrefab = null;
    [Space(5)]
    [SerializeField]private float RateFire = 1.2f;
     [SerializeField]private float FireSpeed = 100;

    private float NextFire = 0;
    private Vector3 lookAt;
    private Transform root;

    void Start()
    {
        NextFire = Time.time + RateFire;
    }

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate()
    {
        float distance = Vector3.Distance(this.transform.position, Target.position);
        
        CannonPosition(distance);
        if (distance > 5)
        {
            FireControll();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void CannonPosition(float distance)
    {
      
        Vector3 lookPosition = Vector3.zero;
        float fixPosY = 4 / distance;
        fixPosY = Mathf.Clamp(fixPosY, 1, 1.8f);
        lookPosition = (distance > 7) ? Target.position : new Vector3(Target.position.x, fixPosY, Target.position.z);
        lookAt = Vector3.Lerp(lookAt, lookPosition, 7 * Time.deltaTime);
        Canom.LookAt(lookAt);
    }

    /// <summary>
    /// 
    /// </summary>
    void FireControll()
    {
        if(Time.time > NextFire)
        {
            NextFire = Time.time + RateFire;
            Fire();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void Fire()
    {
        GameObject ball = Instantiate(BallPrefab, FirePosition.position, FirePosition.localRotation) as GameObject;
        Rigidbody r = ball.GetComponent<Rigidbody>();
        float y = Target.position.y;
        r.AddForce((FirePosition.forward * FireSpeed) * (y - 0.54f), ForceMode.Impulse);
        //If you use a attaker that is instance from enemy, like a bullet,ray,power,etc..
        //then it is recommendable to send the reference of the real enemy / sender to the instance,like this:
        //cache the reference in a var of the instance
        ball.GetComponent<bl_Attaker>().Sender = this.gameObject;
        if (root == null)
        {
            if (GameObject.Find("BallMaster") == null)
            {
                GameObject newgo = new GameObject("BallMaster");
                root = newgo.transform;
            }
            else
            {
                root = GameObject.Find("BallMaster").transform;
            }
        }
        ball.transform.parent = root;
        Destroy(ball, 10);
    }

}