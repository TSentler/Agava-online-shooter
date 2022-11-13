using UnityEngine;
using System.Collections;

public class bl_Attaker : MonoBehaviour {

    [HideInInspector]public GameObject Sender = null;

    private float _damage = 20f;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="c"></param>
    void OnTriggerEnter(Collider c)
    {
        if(c.transform.tag == "Player")
        {
          
            //This is the important that you impliment in your own scripts.
            //No need use the struct 'bl_DamageInfo', just sure of send this two variables:
            //---GameObject Sender---- = the enemy that inflict the damage, 
            //in this case the turrent that shoot this ball.

            //This is the method that use for this example, you can use your own for notify the player
            //that have received damage
            bl_DamageInfo info = new bl_DamageInfo(_damage);
            //Send the sender (enemy) that inflict this damage.
            info.Sender = Sender;

            if(c.TryGetComponent(out bl_DamageCallback damageCallback))
            {
                damageCallback.OnDamage(info);
            }
                
            //And the other important variable is the position of enemy.
            //for this is we need to have a reference of enemy to do the following:
            Sender.SetIndicator();
        }
    }
}