using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    bool readyForPickup = false;
    float timeBetween = 15f;

    public GameObject AButton;
    public GameObject BButton;

    public GameObject LeftKey;
    public GameObject RightKey;
    public GameObject UpKey;
    public GameObject DownKey;

    PlayerHealth health;

    bool oneKey = false;
    int numKeysLost = 0;

    float screenWidth;
    float screenHeight;

    private void Start()
    {
        health = GameObject.Find("Player").GetComponent<PlayerHealth>();
        StartCoroutine(WaitForPickup());

        Camera cam = Camera.main;

        Vector2 screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        Vector2 screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.y - screenBottomLeft.y;
    }

    void Update()
    {
        if (readyForPickup && health.IsAlive())
        {
            oneKey = TestForOneKey();
            GameObject pickup = CalculateOdds();

            SpawnPickup(pickup);

            StartCoroutine(WaitForPickup());
        }
    }

    // allows pickups to be spawned by outside sources (joystick)
    public void SpawnPickup(GameObject pickup)
    {
        // determine where to spawn key
        Vector2 location = CalculateSpawn(GameObject.Find("Player").transform.position);

        // spawn pickup
        GameObject p = Instantiate(pickup, location, Quaternion.identity);
        p.SetActive(true);
        StartCoroutine(DespawnPickup(p));
    }

    IEnumerator DespawnPickup(GameObject p)
    {
        yield return new WaitForSeconds(16f);
        //flashing
        float i = 0;
        while (i < 4)
        {
            if (p)
            {
                if (p.activeSelf)
                {
                    p.SetActive(false);
                    yield return new WaitForSeconds(0.1f);
                    i += 0.1f;
                }
                else
                {
                    p.SetActive(true);
                    yield return new WaitForSeconds(0.2f);
                    i += 0.2f;
                }
            }
            else
            {
                i = 5;
                break;
            }

        }
        Destroy(p);
    }

    bool TestForOneKey()
    {
        numKeysLost = 0;
        if (!health.Left.GetComponent<Key>().alive)
        {
            numKeysLost++;
        }
        if (!health.Right.GetComponent<Key>().alive)
        {
            numKeysLost++;
        }
        if (!health.Up.GetComponent<Key>().alive)
        {
            numKeysLost++;
        }
        if (!health.Down.GetComponent<Key>().alive)
        {
            numKeysLost++;
        }

        return (!health.Left.GetComponent<Key>().alive && !health.Right.GetComponent<Key>().alive) || (!health.Up.GetComponent<Key>().alive && !health.Down.GetComponent<Key>().alive);
    }

    Vector2 CalculateSpawn(Vector2 player)
    {
        // if has a given axis, spawn on that axis
        if (health.Left.GetComponent<Key>().alive)
        {
            return new Vector2(Random.Range((-screenWidth / 2) + 1, (screenWidth / 2) - 1) , player.y);
        }
        else
        {
            return new Vector2(player.x, Random.Range((-screenHeight / 2) + 1, (screenHeight / 2) - 1));
        }
    }

    GameObject CalculateOdds()
    {
        // std odds are 70% key to 30% button
        int value = Random.Range(0, 100);
        if (value < (60 + numKeysLost * 10))
        {
            // determine which key
            if (oneKey)
            {
                // if missing both from a direction, prioritize
                // that direction
                if (!health.Left.GetComponent<Key>().alive && !health.Right.GetComponent<Key>().alive)
                {
                    // spawn left key
                    return LeftKey;
                }
                else
                {
                    // spawn up key
                    return UpKey;
                }
            }
            else
            {
                if (!health.Left.GetComponent<Key>().alive)
                {
                    // spawn left key
                    return LeftKey;
                }
                else if (!health.Up.GetComponent<Key>().alive)
                {
                    // spawn up key
                    return UpKey;
                }
                else if (!health.Right.GetComponent<Key>().alive)
                {
                    // spawn right key
                    return RightKey;
                }
                else
                {
                    // spawn down key
                    return DownKey;
                }
            }
        }
        else
        {
            // determine which button
            // 50-50 chance
            int i = Random.Range(0, 10);
            if ((!health.B.GetComponent<Key>().alive && i < 4) || health.B.GetComponent<Key>().alive)
            {
                // spawn b
                return BButton;
            }
            else
            {
                // spawn a
                return AButton;
            }
        }
    }

    IEnumerator WaitForPickup()
    {
        readyForPickup = false;
        yield return new WaitForSeconds(timeBetween);
        readyForPickup = true;
    }
}
