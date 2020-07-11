using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Battery : MonoBehaviour
{
    GameObject player;

    Vector2 currPos;
    Vector2 playerPos;
    Vector2 unit;
    Vector2 move;
    public float speed = 3;
    public float speedDecreaser = 0.5f;
    
    public float lifespan;          // in seconds
    public float betweenJumps;      // in seconds
    float jumpBuffer;
    bool landed;
    bool calcJump = true;

    bool awake = true; // TODO set to false

    void Start()
    {
        player = GameObject.Find("Player");
        lifespan *= 60;
        betweenJumps *= 60;
        jumpBuffer = betweenJumps;
    }

    
    void Update()
    {
        if (!awake)
        {
            // TODO play spawn animation
            // TODO set awake to true after animation is done
        }
        else
        {
            Jump();
            transform.Translate(move);
            if (lifespan > 0)
            {
                Life();
            }
           // else {
                // TODO play death animation
                // TODO delete battery
           // }
        }

        // flip scale
        transform.localScale = (move.x > 0) ? new Vector3(-1,1,1) : new Vector3(1,1,1);
    }
    
    // accounts for how long battery is alive, and what stage of life it's in
    void Life()
    {
        lifespan--;
        if(lifespan == ((2 * lifespan) / 3))
        {
            speed -= speedDecreaser;
            // Change Sprite to half full.
        } else if (lifespan == (lifespan / 3))
        {
            speed = speedDecreaser;
            // Change sprite to almost empty
        }
    }

    // This just calculates the vector between the battery and the player and moves it toward the player
    void Chase()
    {
        currPos = new Vector2(transform.position.x, transform.position.y);
        playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
        unit = playerPos - currPos;
        unit.Normalize();

        
        // TODO momentum might help make the chasing a little more forgiving
    }

    // TODO avoids other batteries + enemies if we want
  /*  void Avoid()
    {

    } */

    // the actual movement
    void Jump()
    {
        if(jumpBuffer > 0)
            jumpBuffer--;

        if (jumpBuffer == 0)
        {
            if (calcJump)
            {
                Chase();
               // Avoid();
                calcJump = false;
            }
            // TODO play animation
            move = (unit) * speed * Time.deltaTime;
            // TODO if jump animation over, landed = true
            if (landed)
                jumpBuffer = betweenJumps;
            calcJump = true;
        }

    }
}
