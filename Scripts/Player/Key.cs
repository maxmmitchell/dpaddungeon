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
    }

    public void Press()
    {
        sprite.sprite = pressedSprite;
    }

    public void Release()
    {
        sprite.sprite = aliveSprite;
    }

    public void Die()
    {
        alive = false;
        sprite.sprite = deadSprite;
        GetComponent<Collider2D>().enabled = false;
    }

    public void Revive()
    {
        alive = true;
        sprite.sprite = aliveSprite;
        GetComponent<Collider2D>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (alive)
        {
            health.CollidedWith(collision.gameObject, this);
        }
    }
}
