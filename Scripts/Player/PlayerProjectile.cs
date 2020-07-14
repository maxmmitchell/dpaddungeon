using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // destroys other projectiles and self
        if (other.gameObject.layer == 14)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        //TODO damage/kill enemies
        if (other.gameObject.layer == 13)
        {
            //hit enemies
            if (other.gameObject.GetComponent<Battery>())
            {
                other.gameObject.GetComponent<Battery>().Hit();
                Destroy(gameObject);
            }
            else if (other.gameObject.GetComponent<Joystick>())
            {
                other.gameObject.GetComponent<Joystick>().Hit();
                Destroy(gameObject);
            }
        }
    }
}
