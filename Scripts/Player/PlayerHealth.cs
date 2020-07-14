using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    public GameObject flash;

    ScoreManager scoreManager;

    public GameObject respawnText;

    public SpriteRenderer faceSprite;
    public Sprite idleFace;
    public Sprite hurtFace;
    public Sprite hehFace;
    public Sprite deadFace;

    //TODO
    // stuff not respawning on other side

    private void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        if (!IsAlive())
        {
            //one-time events
            if (!respawnText.activeSelf)
            {
                KillKey(A);
                KillKey(B);

                respawnText.SetActive(true);

                //SFX
                AudioManager.instance.Play("SFXDeath");

                //ANIMATION TODO
                faceSprite.sprite = deadFace;

                //stop enemy spawning
                Destroy(FindObjectOfType<EnemyManager>().gameObject);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                FindObjectOfType<PauseMenuUI>().Restart();
            }
        }
    }

    public bool IsAlive()
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
            // kills main character key 
            GameObject.Find(us.name).GetComponent<Key>().Die();

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

            if (IsAlive())
            {
                StartCoroutine(InvincibilityFrames());

                //SFX
                AudioManager.instance.Play("SFXDeath", 2f);
                //face
                StartCoroutine(SwapFace(hurtFace, 2f));
            }
        }

        // if on pickup layer, collect pickup
        if (other.layer == 11 && IsAlive())
        {
            CollectPickup(other.gameObject);
        }
    }

    IEnumerator InvincibilityFrames()
    {
        invincible = true;
        StartCoroutine(InvincibilityFlash());
        yield return new WaitForSeconds(2f);
        invincible = false;
    }

    IEnumerator InvincibilityFlash()
    {
        float i = 0;
        Color c;

        // flash white
        flash.SetActive(true);
        yield return null;
        flash.SetActive(false);

        SpriteRenderer[] allSprites = GetComponentsInChildren<SpriteRenderer>();
        // blink
        while (i < 1.9f)
        {
            c = GetComponent<SpriteRenderer>().color;
            if (c.r == 0)
            {
                foreach (SpriteRenderer s in allSprites)
                {
                    s.color = new Color(1, 1, 1, 1);
                }
                yield return new WaitForSeconds(0.2f);
                i += 0.2f;
            }
            else
            {
                foreach (SpriteRenderer s in allSprites)
                {
                    s.color = new Color(0, 0, 0, 0);
                }
                yield return new WaitForSeconds(0.1f);
                i += 0.1f;
            }           
        }

        foreach (SpriteRenderer s in allSprites)
        {
            s.color = new Color(1, 1, 1, 1);
        }
    }

    void CollectPickup(GameObject pickup)
    {
        int score = 0;
        // every pickup is named Drop____
        // after first four letters, indicates type of pickup
        string name = pickup.name.Substring(4, 1);

        // which key/button to revive
        GameObject key = A;

        if (name == "U")
        {
            key = Up;
            score += 500;
        }
        else if (name == "L")
        {
            //Left.GetComponent<Key>().Revive();
            key = Left;
            score += 500;
        }
        else if (name == "R")
        {
            //Right.GetComponent<Key>().Revive();
            key = Right;
            score += 500;
        }
        else if (name == "D")
        {
            key = Down;
            score += 500;
        }
        else if (name == "A")
        {
            //A.GetComponent<Key>().Revive();
            SwapFace(hehFace, 2f);
            key = A;
            score += 1000;
            GetComponent<PlayerCombat>().shooting = false;
            GetComponent<PlayerCombat>().shotReset = true;
            A.GetComponent<Key>().Release();
        }
        else if (name == "B")
        {
            //B.GetComponent<Key>().Revive();
            key = B;
            score += 1000;
            GetComponent<PlayerCombat>().shielding = false;
            GetComponent<PlayerCombat>().shieldReset = true;
            B.GetComponent<Key>().Release();
        }

        // Revives across all ghosts
        key.GetComponent<Key>().Revive();

        foreach (Transform g in GetComponent<ScreenWrapping>().ghosts)
        {
            g.Find(key.name).GetComponent<Key>().Revive();
        }

        //SFX
        AudioManager.instance.Play("SFXDeath", 2f);

        scoreManager.AddScore(score, transform.position);
        Destroy(pickup);
    }

    // swaps to new face for a few sec, then back to original
    public IEnumerator SwapFace(Sprite newFace, float time)
    {
        faceSprite.sprite = newFace;
        yield return new WaitForSeconds(time);
        faceSprite.sprite = idleFace;
    }
}
