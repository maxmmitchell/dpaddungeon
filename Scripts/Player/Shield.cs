using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    // MAKE THIS WORK TODO 

    PlayerMovement mov;
    void Start()
    {
        mov = GetComponentInParent<PlayerMovement>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // repel projectiles
        if (other.gameObject.layer == 14)
        {
            Destroy(other.gameObject);
        }
    }
}
