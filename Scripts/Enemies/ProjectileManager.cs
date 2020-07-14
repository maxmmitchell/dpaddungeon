using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        // if on projectile layer or player projectile layer
        if (other.gameObject.layer == 14 || other.gameObject.layer == 9)
        {
            Destroy(other.gameObject);
        }
    }
}
