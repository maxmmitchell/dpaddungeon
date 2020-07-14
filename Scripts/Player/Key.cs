using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public bool alive;
    public Sprite aliveSprite;
    public Sprite pressedSprite;
    public Sprite deadSprite;

    PlayerHealth health;

    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = aliveSprite;

        // if ghost, then check grandparent for playerhealth
        if (GetComponentInParent<Ghost>())
        {
            health = GetComponentInParent<Ghost>().health;
        }
        else
        {
            health = GetComponentInParent<PlayerHealth>();
        }

        alive = true;

        // a and b buttons start dead!
        if (name.Substring(0, 1) == "A" || name.Substring(0, 1) == "B")
        {
            Die();
        }
    }

    public void Press()
    {
        sprite.sprite = pressedSprite;
    }

    public void Release()
    {
        if (alive)
        {
            sprite.sprite = aliveSprite;
        }
        else
        {
            sprite.sprite = deadSprite;
        }
    }

    public void Die()
    {
        alive = false;
        sprite.sprite = deadSprite;
        GetComponent<Collider2D>().enabled = false;
    }

    public void Revive()
    {
        if (!alive)
        {
            alive = true;
            sprite.sprite = aliveSprite;
            GetComponent<Collider2D>().enabled = true;

            // takes into account if recharged or not
            if (GetComponentInParent<PlayerCombat>())
            {
                if (name.Substring(0, 1) == "B" && GetComponentInParent<PlayerCombat>().shielding)
                {
                    Press();
                }
                else if (name.Substring(0, 1) == "A" && GetComponentInParent<PlayerCombat>().shooting)
                {
                    Press();
                }
            }
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (alive)
        {
            health.CollidedWith(collision.gameObject, this);
        }
    }
}
