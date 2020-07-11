using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject Left;
    public GameObject Right;
    public GameObject Up;
    public GameObject Down;

    public GameObject A;
    public GameObject B;

    public LayerMask Death;

    public bool invincible = false;

    void Update()
    {
        if (!IsAlive())
        {
            // death sfx, animation, procedure, etc.
        }
    }

    bool IsAlive()
    {
        return Left.GetComponent<Key>().alive || Right.GetComponent<Key>().alive || Up.GetComponent<Key>().alive || Down.GetComponent<Key>().alive;
    }

    void KillKey(GameObject key)
    {
        key.GetComponent<Key>().Die();
    }

    void ReviveKey(GameObject key)
    {
        key.GetComponent<Key>().Die();
    }

    public void CollidedWith(GameObject other, Key us)
    {
        // if on one of the layers of death
        if ((1 << other.layer & Death) != 0 && !invincible)
        {
            StartCoroutine(InvincibilityFrames());
            us.Die();
            
            // kills same key on all ghosts so we are consistent
            foreach (Transform g in GetComponent<ScreenWrapping>().ghosts)
            {
                g.Find(us.name).GetComponent<Key>().Die();
            }

            // destroy projectile, if projectile hit you
            if (other.layer == 14)
            {
                Destroy(other);
            }
        }

        // if on pickup layer, collect pickup
        if (other.layer == 11)
        {
            CollectPickup();
        }
    }

    IEnumerator InvincibilityFrames()
    {
        invincible = true;
        yield return new WaitForSeconds(0.3f);
        invincible = false;
    }

    void CollectPickup()
    {
        // TODO
    }
}
