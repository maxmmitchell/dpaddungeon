using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Battery : MonoBehaviour
{
    GameObject player;

    Animator anim;
    string jumpAnim = "Jump_1";

    Vector2 currPos;
    Vector2 playerPos;
    Vector2 unit;
    Vector2 move;
    public float speed = 3;
    public float speedDecreaser = 0.5f;

    public float lifespan;          // in seconds
    float lifeleft;

    public float betweenJumps;      // in seconds
    float jumpBuffer;
    bool landed;
    bool jumping = false;
    bool inAir = false;

    bool awake = false;

    public Collider2D trackRange;
    public Collider2D body;

    public bool big;
    ProjectileLauncher launcher;

    void Start()
    {
        player = GameObject.Find("Player");
        jumpBuffer = betweenJumps;

        anim = GetComponent<Animator>();
        //SFX
        AudioManager.instance.Play("SFXSpawn");

        lifeleft = lifespan;

        if (big)
        {
            launcher = GetComponent<ProjectileLauncher>();
            StartCoroutine(launcher.LaunchTarget());
        }
    }


    void Update()
    {
        if (awake)
        {
            if (lifeleft > 0)
            {
                if (!jumping)
                {
                    StartCoroutine(Jump());
                }
                if (inAir)
                {
                    transform.Translate(move);
                }

                Life();
            }
            else if (!anim.GetBool("Die"))
            {
                //TODO SFX
                anim.SetBool("Die", true);
                GetComponent<Collider2D>().enabled = false;
                inAir = false;
            }
        }

        // flip scale
        transform.localScale = (move.x > 0) ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 10 is the player layer
        if (other.gameObject.layer == 10 && other.IsTouching(body) && !other.GetComponentInParent<PlayerHealth>().invincible)
        {
            lifeleft = 0.2f;
        }
    }

    void WakeUp()
    {
        awake = true;
        GetComponent<Collider2D>().enabled = true;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Landed()
    {
        jumping = false;
        inAir = false;
    }

    // accounts for how long battery is alive, and what stage of life it's in
    void Life()
    {
        lifeleft -= Time.deltaTime;
        if (lifeleft <= ((2 * lifespan) / 3) && lifeleft > (lifespan / 3))
        {
            //speed -= speedDecreaser;
            // Change Sprite to half full.
            jumpAnim = "Jump_2";
        }
        else if (lifeleft <= (lifespan / 3) && lifeleft > 0)
        {
            //speed = speedDecreaser;
            // Change sprite to almost empty
            jumpAnim = "Jump_3";
        }
    }

    // This just calculates the vector between the battery and the player and moves it toward the player
    void Chase()
    {
        currPos = new Vector2(transform.position.x, transform.position.y);

        Collider2D[] playerCols = player.GetComponentsInChildren<Collider2D>();

        playerPos = new Vector2(UnityEngine.Random.Range(-35, 36), UnityEngine.Random.Range(-19, 20));

        foreach (Collider2D c in playerCols)
        {
            if (trackRange.IsTouching(c))
            {
                playerPos = new Vector2(player.transform.position.x, player.transform.position.y);
                break;
            }
        }



        unit = playerPos - currPos;
        unit.Normalize();


        // TODO momentum might help make the chasing a little more forgiving
    }

    // TODO avoids other batteries + enemies if we want
    /*  void Avoid()
      {
      } */

    // the actual movement
    IEnumerator Jump()
    {
        jumping = true;
        yield return new WaitForSeconds(betweenJumps);
        inAir = true;
        // play animation
        anim.SetTrigger(jumpAnim);

        Chase();
        // Avoid();
        move = (unit) * speed * Time.deltaTime;
    }

    public void Hit()
    {
        lifeleft -= lifespan / 2;
    }
}
