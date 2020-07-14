using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerHealth health;
    Rigidbody2D body;

    Vector2 direction;
    public float speed = 2;

    void Start()
    {
        health = GetComponent<PlayerHealth>();
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        TakeInput();

        Move();
    }

    void TakeInput()
    {
        // collect input
        if (health.Left.GetComponent<Key>().alive)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                PressKey(health.Left, Vector2.left);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                ReleaseKey(health.Left, Vector2.right);
            }
        }
        // if holding down this key when it dies
        else if (direction.x < 0)
        {
            //direction.x = 0;
            ReleaseKey(health.Left, Vector2.right);
        }


        if (health.Right.GetComponent<Key>().alive)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                PressKey(health.Right, Vector2.right);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                ReleaseKey(health.Right, Vector2.left);
            }
        }
        // if holding down this key when it dies
        else if (direction.x > 0)
        {
            //direction.x = 0;
            ReleaseKey(health.Right, Vector2.left);
        }


        if (health.Up.GetComponent<Key>().alive)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                PressKey(health.Up, Vector2.up);
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                ReleaseKey(health.Up, Vector2.down);
            }
        }
        // if holding down this key when it dies
        else if (direction.y > 0)
        {
            //direction.y = 0;
            ReleaseKey(health.Up, Vector2.down);
        }


        if (health.Down.GetComponent<Key>().alive)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                PressKey(health.Down, Vector2.down);
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                ReleaseKey(health.Down, Vector2.up);
            }
        }
        // if holding down this key when it dies
        else if (direction.y < 0)
        {
            //direction.y = 0;
            ReleaseKey(health.Down, Vector2.up);
        }

    }

    void Move()
    {
        // normalize direction vector so speed is constant
        body.velocity = direction.normalized * speed;
    }

    void PressKey(GameObject key, Vector2 velocity)
    {
        key.GetComponent<Key>().Press();
        direction += velocity;
    }

    void ReleaseKey(GameObject key, Vector2 velocity)
    {
        key.GetComponent<Key>().Release();
        direction += velocity;
    }

}
