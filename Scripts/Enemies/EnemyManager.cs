using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // PREFABS
    public GameObject battery;
    public GameObject bigbee;
    public GameObject lightbulb;
    public GameObject plugTOP;
    public GameObject plugBOT;
    public GameObject plugLEF;
    public GameObject plugRIG;
    public GameObject joystick;
    //  //  //  //  //  //  //  //

    public float screenWidth;
    public float screenHeight;

    PlayerHealth health;
    GameObject player;
    void Start()
    {
        Camera cam = Camera.main;

        Vector2 screenBottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z));
        Vector2 screenTopRight = cam.ViewportToWorldPoint(new Vector3(1, 1, transform.position.z));

        screenWidth = screenTopRight.x - screenBottomLeft.x;
        screenHeight = screenTopRight.y - screenBottomLeft.y;

        health = FindObjectOfType<PlayerHealth>();
        player = GameObject.Find("Player");

        StartCoroutine(StageOne());
    }


    // spawns mob at given position
    public void SpawnMob(GameObject mob, Vector2 pos)
    {
        Instantiate(mob, pos, Quaternion.identity).SetActive(true);
    }

    // overflow, spawns mob randomly
    public void SpawnMob(GameObject mob)
    {
        SpawnMob(mob, new Vector2(Random.Range((-screenWidth / 2) + 1, (screenWidth / 2) - 1), Random.Range((-screenHeight / 2) + 1, (screenHeight / 2) - 1)));
    }

    IEnumerator StageOne()
    {
        StartCoroutine(SpawnBatteries(5, 2));
        yield return new WaitForSeconds(10f);
        SpawnRandomPlug();
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnLightbulbs(1, 4));
        yield return new WaitForSeconds(8f);

        SpawnRandomPlug();
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnBatteries(6, 2));
        yield return new WaitForSeconds(12f);
        SpawnRandomPlug();
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnLightbulbs(2, 4));
        yield return new WaitForSeconds(12f);
        SpawnRandomPlug();
        yield return new WaitForSeconds(1f);

        StartCoroutine(SpawnBigbees(5, 2));
        yield return new WaitForSeconds(8f);
        StartCoroutine(SpawnBatteries(6, 2));
        yield return new WaitForSeconds(12f);
        SpawnRandomPlug();
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnLightbulbs(2, 3));
        SpawnRandomPlug();
        yield return new WaitForSeconds(9f);
        SpawnRandomPlug();
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnBatteries(7, 1.6f));
        yield return new WaitForSeconds(14f);
        SpawnRandomPlug();
        yield return new WaitForSeconds(1f);
        StartCoroutine(SpawnBigbees(5, 2));
        yield return new WaitForSeconds(16f);

        StartCoroutine(SpawnPlugs(4, 1f));
        yield return new WaitForSeconds(8f);
        StartCoroutine(SpawnLightbulbs(3, 5));
        yield return new WaitForSeconds(9f);
        StartCoroutine(SpawnBigbees(7, 3));
        yield return new WaitForSeconds(16f);
        SpawnRandomPlug();
        StartCoroutine(SpawnBatteries(9, 1.7f));
        yield return new WaitForSeconds(16f);
        SpawnRandomPlug();
        StartCoroutine(SpawnLightbulbs(4, 6));
        yield return new WaitForSeconds(16f);
        StartCoroutine(SpawnBigbees(7, 3));
        yield return new WaitForSeconds(16f);
        StartCoroutine(SpawnLightbulbs(2, 3));
        SpawnRandomPlug();
        yield return new WaitForSeconds(9f);
        SpawnRandomPlug();
        StartCoroutine(SpawnPlugs(4, 1f));
        yield return new WaitForSeconds(8f);

        //BIG BOSS
        SpawnMob(joystick, Vector2.zero);
    }

    public IEnumerator SpawnBatteries(int num, float delay)
    {
        for (int i = 0; i < num; i++)
        {
            SpawnMob(battery);
            yield return new WaitForSeconds(delay);
        }
    }

    public IEnumerator SpawnBigbees (int num, float delay)
    {
        for (int i = 0; i < num; i++)
        {
            SpawnMob(bigbee);
            yield return new WaitForSeconds(delay);
        }
    }

    public IEnumerator SpawnLightbulbs(int num, float delay)
    {
        for (int i = 0; i < num; i++)
        {
            SpawnMob(lightbulb);
            yield return new WaitForSeconds(delay);
        }
    }

    public IEnumerator SpawnPlugs(int num, float delay)
    {
        for (int i = 0; i < num; i++)
        {
            SpawnRandomPlug();
            yield return new WaitForSeconds(delay);
        }
    }

    void SpawnRandomPlug()
    {
        // dont zap player in a way they can't dodge
        // if missing both from a direction, prioritize
        // the opposite
        int r = Random.Range(0, 4);
        Vector2 pos;
        GameObject mob;

        if (!health.Left.GetComponent<Key>().alive && !health.Right.GetComponent<Key>().alive || (r < 2 && (health.Up.GetComponent<Key>().alive || health.Down.GetComponent<Key>().alive)))
        {
            // attack from side
            pos.y = player.transform.position.y;

            //LEF
            if (r == 0)
            {
                pos.x = -screenWidth / 2 + 2;
                mob = plugLEF;
            }
            //RIG
            else
            {
                pos.x = screenWidth / 2 - 2;
                mob = plugRIG;
            }
        }
        else
        {
            // attack from top/bottom

            pos.x = player.transform.position.x;

            //TOP
            if (r == 3)
            {
                pos.y = screenHeight / 2 - 2;
                mob = plugTOP;
            }
            //BOT
            else
            {
                pos.y = -screenHeight / 2 + 2;
                mob = plugBOT;
            }
        }
        SpawnMob(mob, pos);
    }
}
