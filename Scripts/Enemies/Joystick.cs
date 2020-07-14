using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    EnemyManager manager;
    PickupManager PM;

    Animator anim;
    public Animator face;
    Rigidbody2D body;
    public GameObject flash;

    int HP = 100;
    public Sprite defeatSprite;

    // DASH
    Vector2 dashDir = Vector2.left;
    bool offScreen = false;
    float speed = 30f;

    //JUMP
    public GameObject silhouette;
    float airTime = 5f;
    GameObject player;

    public float screenWidth;
    public float screenHeight;

    void Start()
    {
        Camera cam = Camera.main;

        Vector2 screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        Vector2 screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.y - screenBottomLeft.y;

        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        PM = FindObjectOfType<PickupManager>();
        manager = FindObjectOfType<EnemyManager>();
        player = GameObject.Find("Player");

        // spawn animation / SFX
        StartCoroutine(Spawn());
        // RNG direction to dash
        // dash telegraph 
        // dash attack
    }

    // start at top of screen, dash down towards center and stop, then awaken
    IEnumerator Spawn()
    {
        AudioManager.instance.Stop(AudioManager.instance.musicPlaying);
        AudioManager.instance.Play("MusicFourFruits");

        // start above screen
        // TODO flash warning graphic
        transform.position = new Vector2(0, 25);
        body.velocity = Vector2.down * 15f;
        while (!(transform.position.x * Vector2.down.x >= 0 && transform.position.y * Vector2.down.y >= 0))
        {
            // keep moving in same direction till arrive at roughly the center
            yield return null;
        }
        // stop at center
        body.velocity = Vector2.zero;
        face.SetTrigger("Awake");
    }

    // called after face wakeup animation plays. determines which attack to use
    public void WakeUp()
    {
        TelegraphJump();
    }

    #region JUMPATTACK

    void TelegraphJump()
    {
        face.SetTrigger("Jump");
    }

    public void BodyJump()
    {
        anim.SetTrigger("Jump");
    }

    IEnumerator Jump()
    {
        //called after jump animation is performed. move joystick up
        // while keeping silhouette down
        silhouette.SetActive(true);
        silhouette.GetComponent<Animator>().SetTrigger("Jump");
        Vector2 pos = silhouette.transform.position;

        body.velocity = Vector2.up * 25f;
        body.GetComponent<Collider2D>().enabled = false;
        
        while (transform.position.y < 30)
        {
            silhouette.transform.position = pos;
            yield return null;
        }
        // reached peak of jump
        // silhouette should now be on tracking animation
        silhouette.GetComponent<Animator>().SetTrigger("Air");

        body.velocity = Vector2.zero;
        silhouette.transform.position = pos;
        StartCoroutine(Track());
    }

    IEnumerator Track()
    {
        float i = 0;
        while (i < airTime)
        {
            Vector2 currPos = new Vector2(silhouette.transform.position.x, silhouette.transform.position.y);
            Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.y);

            Vector2 unit = playerPos - currPos;
            unit.Normalize();
            silhouette.transform.Translate((unit) * 6 * Time.deltaTime);

            yield return null;
            i += Time.deltaTime;
        }
        StartCoroutine(Landing());
    }

    IEnumerator Landing()
    {
        transform.position = new Vector2(silhouette.transform.position.x, transform.position.y);
        silhouette.transform.localPosition = new Vector3(0, silhouette.transform.localPosition.y, silhouette.transform.localPosition.z);
        //animation for falling
        silhouette.GetComponent<Animator>().SetTrigger("Fall");

        Vector2 pos = silhouette.transform.position;
        body.velocity = Vector2.down * 35f;

        // account for pivot difference
        while (transform.position.y > pos.y + 5)
        {
            silhouette.transform.position = pos;
            yield return null;
        }
        body.velocity = Vector2.zero;
        //animation for landing
        anim.SetTrigger("Land");
        body.GetComponent<Collider2D>().enabled = true;
        silhouette.transform.position = pos;
        silhouette.SetActive(false);
        //launch projectiles
        StartCoroutine(manager.SpawnBigbees(5, 0.1f));
        //camera shake
        StartCoroutine(FindObjectOfType<CameraShake>().Shake(1f, 0.2f));
        // TODO SFX
    }

    //called at end of landing animation
    void BackToBase()
    {
        TelegraphDash();
    }


    #endregion

    #region DASHATTACK

    void TelegraphDash()
    {
        // pick direction to dash
        int r = Random.Range(0, 4);
        // animate according telegraph animation
        if (r == 0)
        {
            //left
            face.SetTrigger("DashLeft");
            dashDir = Vector2.left;
        }
        else if (r == 1)
        {
            //right
            face.SetTrigger("DashRight");
            dashDir = Vector2.right;
        }
        else if (r == 2)
        {
            //up
            face.SetTrigger("DashUp");
            dashDir = Vector2.up;
        }
        else if (r == 3)
        {
            //down
            face.SetTrigger("DashDown");
            dashDir = Vector2.down;
        }
    }

    public void Dash()
    {
        //SFX
        AudioManager.instance.Play("SFXBlast");

        //called from animator event at end of telegraph
        //move joystick
        body.velocity = dashDir * speed;

        //spawn batteries in wake
        StartCoroutine(BatterySpawn());

        //check for dashing off screen, and wrap to other side of screen
        StartCoroutine(CheckOffScreen());
    }

    IEnumerator BatterySpawn()
    {
        yield return new WaitForSeconds(0.2f);
        while (body.velocity != Vector2.zero)
        {
            manager.SpawnMob(manager.battery, new Vector2(transform.position.x - dashDir.x * 2 + Random.Range(-4, 5), 
                                                          transform.position.y - dashDir.y * 2 + Random.Range(-4, 5)));
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator CheckOffScreen()
    {
        while (!offScreen)
        {
            if (transform.position.x < -screenWidth / 2 || transform.position.x > screenWidth / 2 || 
                transform.position.y < -screenHeight / 2 || transform.position.y > screenHeight / 2)
            {
                offScreen = true;
                StartCoroutine(DashOnScreen());
            }
            yield return null;
        }
        offScreen = false;
    }

    IEnumerator DashOnScreen()
    {
        // teleport to opposite side of screen
        // since we only move in x or y direction
        transform.position = -(transform.position * dashDir * dashDir) + new Vector2(dashDir.y * transform.position.x, dashDir.x * transform.position.y);

        while (!(transform.position.x * dashDir.x >= 0 && transform.position.y * dashDir.y >= 0))
        {
            // keep moving in same direction till arrive at roughly the center
            yield return null;
        }
        // stop at center
        body.velocity = Vector2.zero;
        // start dizzy indicator
        face.SetBool("Dizzy", true);
        PM.SpawnPickup(PM.AButton);
        // give player time to attack, then return to base
        yield return new WaitForSeconds(15f);
        face.SetBool("Dizzy", false);
    }
    #endregion

    public void Hit()
    {
        HP -= 5;
        StartCoroutine(DamageFlash());

        if (HP <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageFlash()
    {
        float i = 0;
        while (i < 1)
        {
            if (flash.activeSelf)
            {
                flash.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                i += 0.1f;
            }
            else
            {
                flash.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                i += 0.2f;
            }
        }
        flash.SetActive(false);
    }

    void Die()
    {
        face.gameObject.SetActive(false);
        Destroy(gameObject.GetComponent<Animator>());
        GetComponent<SpriteRenderer>().sprite = defeatSprite;

        StartCoroutine(Victory());
    }

    IEnumerator Victory()
    {
        FindObjectOfType<PlayerHealth>().invincible = true;
        PlayerPrefs.SetInt("Score", FindObjectOfType<ScoreManager>().score);
        yield return new WaitForSeconds(2f);
        FindObjectOfType<SceneLoader>().LoadNextScene("End");
    }

}
